using System.Text.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        int uang;
        int totalBiaya = 0;
        BankTransferData data = new BankTransferData();

        if (data.config.lang == "en")
        {
            Console.WriteLine("Please insert the amount of money to transfer: ");
            uang = Convert.ToInt32(Console.ReadLine());
            if (uang == data.config.transfer.treshold)
            {
                totalBiaya = uang + data.config.transfer.low_fee;
                Console.WriteLine("Transfer Fee ", data.config.transfer.low_fee);
                
            } else if (uang > data.config.transfer.treshold)
            {
                totalBiaya = uang + data.config.transfer.high_fee;
                Console.WriteLine("Transfer Fee ", data.config.transfer.high_fee);

            }

            Console.WriteLine("Total Amount ", totalBiaya);
            Console.WriteLine("Select Transfer Method: ");
            for (int i = 0; i < data.config.method.Length; i++)
            {
                Console.WriteLine(data.config.method[i]);
            }
        } else if (data.config.lang == "id")
        {
            Console.WriteLine("Masukkan jumlah uang yang akan di-transfer: ");
            uang = Convert.ToInt32(Console.ReadLine());
            if (uang == data.config.transfer.treshold)
            {
                totalBiaya = uang + data.config.transfer.low_fee;
                Console.WriteLine("Biaya Transfer ", data.config.transfer.low_fee);
            }
            else if (uang > data.config.transfer.treshold)
            {
                totalBiaya = uang + data.config.transfer.high_fee;
                Console.WriteLine("Biaya Transfer ", data.config.transfer.high_fee);
            }

            Console.WriteLine("Total Biaya ", totalBiaya);
            Console.WriteLine("Pilih Metode Tranfser: ");
            for (int i = 0; i < data.config.method.Length; i++)
            {
                Console.WriteLine(data.config.method[i]);
            }
        }
    }
}

public class Transfer
{
    public int treshold { get; set; }
    public int low_fee { get; set; }
    public int high_fee { get; set; }

    public Transfer(int treshold, int low_fee, int high_fee)
    {
        this.treshold = treshold;
        this.low_fee = low_fee;
        this.high_fee = high_fee;
    }
}

public class Confirmation
{
    public string en { get; set; }
    public string id { get; set; }

    public Confirmation(string en, string id)
    {
        this.en = en;
        this.id = id;
    }
}

public class BankTransferConfig
{
    public string lang { get; set; }
    
    public Transfer transfer;
    public string[] method { get; set; }
    
    public Confirmation confirmation;

    public BankTransferConfig() { }
    public BankTransferConfig(string lang, Transfer transfer, string[] method, Confirmation confirmation)
    {
        this.lang = lang;
        this.transfer = transfer;
        this.method = method;
        this.confirmation = confirmation;
    }
}

public class BankTransferData
{
    public BankTransferConfig config;
    public const string filePath = @"bank_transfer_config.json";

    public BankTransferData()
    {
        try
        {
            ReadConfigJson();
        }
        catch (Exception)
        {
            SetDefault();
            WriteNewConfig();
        }
    }

    private BankTransferConfig ReadConfigJson()
    {
        string jsonData = File.ReadAllText(filePath);
        config = JsonSerializer.Deserialize<BankTransferConfig>(jsonData);
        return config;
    }

    private void SetDefault()
    {
        Transfer transfer = new Transfer(1, 1, 1);
        string[] paymentMethods = {"RTO", "SKN", "RTGS", "BI FAST"};
        Confirmation confirmation = new Confirmation("yes", "ya");
        config = new BankTransferConfig("en", transfer, paymentMethods, confirmation);
    }

    private void WriteNewConfig()
    {
        JsonSerializerOptions opts = new JsonSerializerOptions()
        {
            WriteIndented = true
        };

        string jsonString = JsonSerializer.Serialize(config, opts);
        File.WriteAllText(filePath, jsonString);
    }
}