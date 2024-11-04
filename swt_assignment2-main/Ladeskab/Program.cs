using Ladeskab;

Display display = new Display();
Door door = new Door();
LogFile log = new LogFile();
RFID_Reader reader = new RFID_Reader();
USBCharger usbCharger = new USBCharger();

ChargeControl chargeControl = new ChargeControl(usbCharger, display);

StationControl stationControl = new StationControl(door, reader, display, log, chargeControl);

bool finish = false;
do
{
    string input;
    Console.WriteLine("Indtast E, O, C, R: ");
    input = Console.ReadLine();
    if (string.IsNullOrEmpty(input)) continue;

    switch (input[0])
    {
        case 'E':
            finish = true;
            break;

        case 'O':
            door.OpenDoor();
            break;

        case 'C':
            door.CloseDoor();
            break;

        case 'R':
            System.Console.WriteLine("Indtast RFID id: ");
            string idString = System.Console.ReadLine();

            int id = Convert.ToInt32(idString);
            reader.setRFID(id);
            break;

        default:
            break;
    }

} while (!finish);
