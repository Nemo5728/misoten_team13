using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BLEManager : SingletonMonoBehaviour<BLEManager>
{
    enum States
    {
        None,
        Scan,
        ScanRSSI,
        Connect,
        Read,
        Disconnect
    }

    public string   DeviceName          = "MST1301";
    string          ServiceUUID         = "11223344-5566-7788-9900-AABBCCDDEE00";
    string          ReadCharacteristic  = "12345678-9012-3456-7890-123456789011";

    BluetoothDeviceScript bds;

    string _message;

    public string GetMessage(){
        return _message;
    }

    private bool _connected = false;
    private float _timeout = 10f;
    private States _state = States.None;
    private string _deviceAddress;
    private bool _foundReadID = false;
    private byte[] _dataBytes = null;
    private bool _rssiOnly = false;
    private int _rssi = 0;


    // Use this for initialization
    void Start()
    {
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {

            SetState(States.Scan, 0.1f);

        }, (error) =>
        {

            BluetoothLEHardwareInterface.Log("Error during initialize: " + error);
        });

        bds = GameObject.Find("BluetoothLEReceiver").GetComponent<BluetoothDeviceScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_timeout > 0f)
        {
            _timeout -= Time.deltaTime;
            if (_timeout <= 0f)
            {
                _timeout = 0f;

                switch (_state)
                {
                    case States.None:
                        break;

                    case States.Scan:
                        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
                        {

                            // if your device does not advertise the rssi and manufacturer specific data
                            // then you must use this callback because the next callback only gets called
                            // if you have manufacturer specific data

                            if (!_rssiOnly)
                            {
                                if (name.Contains(DeviceName))
                                {
                                    BluetoothLEHardwareInterface.StopScan();

                                    // found a device with the name we want
                                    // this example does not deal with finding more than one
                                    _deviceAddress = address;
                                    SetState(States.Connect, 0.5f);
                                }
                            }

                        }, (address, name, rssi, bytes) =>
                        {

                            // use this one if the device responses with manufacturer specific data and the rssi

                            if (name.Contains(DeviceName))
                            {
                                if (_rssiOnly)
                                {
                                    _rssi = rssi;
                                }
                                else
                                {
                                    BluetoothLEHardwareInterface.StopScan();

                                    // found a device with the name we want
                                    // this example does not deal with finding more than one
                                    _deviceAddress = address;
                                    SetState(States.Connect, 0.5f);
                                }
                            }

                        }, _rssiOnly); // this last setting allows RFduino to send RSSI without having manufacturer data

                        if (_rssiOnly)
                            SetState(States.ScanRSSI, 0.5f);
                        break;

                    case States.ScanRSSI:
                        break;

                    case States.Connect:
                        // set these flags
                        _foundReadID = false;

                        // note that the first parameter is the address, not the name. I have not fixed this because
                        // of backwards compatiblity.
                        // also note that I am note using the first 2 callbacks. If you are not looking for specific characteristics you can use one of
                        // the first 2, but keep in mind that the device will enumerate everything and so you will want to have a timeout
                        // large enough that it will be finished enumerating before you try to subscribe or do any other operations.
                        BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) =>
                        {
                            BluetoothLEHardwareInterface.Log("connectToPeripheral");


                            if (IsEqual(serviceUUID, ServiceUUID))
                            {
                                _foundReadID = _foundReadID || IsEqual(characteristicUUID, ReadCharacteristic);

                                // if we have found both characteristics that we are waiting for
                                // set the state. make sure there is enough timeout that if the
                                // device is still enumerating other characteristics it finishes
                                // before we try to subscribe
                                if (_foundReadID)
                                {
                                    _connected = true;
                                    SetState(States.Read, 2f);
                                }
                            }
                        });
                        break;
                    case States.Read:
                        BluetoothLEHardwareInterface.ReadCharacteristic(_deviceAddress, ServiceUUID, ReadCharacteristic, (message, messageChar) =>
                        {
                            _message = bds._message;
                        });

                        _timeout = 0.1f;

                        break;

                    case States.Disconnect:
                        if (_connected)
                        {
                            BluetoothLEHardwareInterface.DisconnectPeripheral(_deviceAddress, (address) =>
                            {
                                BluetoothLEHardwareInterface.DeInitialize(() =>
                                {

                                    _connected = false;
                                    _state = States.None;
                                });
                            });
                        }
                        else
                        {
                            BluetoothLEHardwareInterface.DeInitialize(() =>
                            {

                                _state = States.None;
                            });
                        }
                        break;
                }
            }
        }
    }

    bool IsEqual(string uuid1, string uuid2)
    {
        if (uuid1.Length == 4)
            uuid1 = FullUUID(uuid1);
        if (uuid2.Length == 4)
            uuid2 = FullUUID(uuid2);


        return (uuid1.CompareTo(uuid2) == 0);
    }

    string FullUUID(string uuid)
    {
        return "0000" + uuid + "-0000-1000-8000-00805f9b34fb";
    }

    void SetState(States newState, float timeout)
    {
        _state = newState;
        _timeout = timeout;
    }
}