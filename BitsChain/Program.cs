using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

public class Transaction
{
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public double Amount { get; set; }
    public DateTime TransactionTime { get; set; }

}

public class Block
{
    public int nonce { get; set; }
    public Block()
    {
        Transactions=new List<Transaction>();
    }
    public List<Transaction> Transactions { get; set; }
    public void AddTransaction(Transaction transaction)
    {
        this.Transactions.Add(transaction);
    }
    public string Hash { get; set; }
    public string PreviousHash { get; set; }
}
public class BracCoin
{
    
    public BracCoin()
    {
        OpenBlock=new Block();
        Chain = new List<Block>(); 
    }
    public Block OpenBlock { get; set; }
    public List<Block> Chain { get; set; }
    public void AddTransaction(Transaction transaction)
    {
        OpenBlock.AddTransaction(transaction);
    }
    public string CloseBlock(Block OpenBlock)
    {
        OpenBlock.Hash = HashGenerator.GenerateSHA256Hash(OpenBlock);
        if(Chain.Count>0)
            OpenBlock.PreviousHash = Chain[Chain.Count - 1].Hash;
        Chain.Add(OpenBlock);
        string hash = OpenBlock.Hash;
        OpenBlock = new Block();
        return hash;
    }
}

    public class HashGenerator
{
    public static string GenerateSHA256Hash<T>(T obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj), "Input object cannot be null.");

        // Serialize the object to JSON
        string json = JsonConvert.SerializeObject(obj);

        // Compute the SHA256 hash
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            byte[] hashBytes = sha256.ComputeHash(bytes);

            // Convert the hash bytes to a hex string
            StringBuilder hashStringBuilder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                hashStringBuilder.Append(b.ToString("x2"));
            }

            return hashStringBuilder.ToString();
        }
    }
}

// Example usage
class Program
{
    static void Main()
    {
        //var exampleObject = new { Name = "John Done", Age = 30, Country = "USA" };
        //string hash = HashGenerator.GenerateSHA256Hash("Hello World");
        //Console.WriteLine($"SHA256 Hash: {hash}");
        BracCoin bracCoin = new BracCoin();
        Block block = new Block();
        //bracCoin.
        Transaction tx = new Transaction();
        tx.Sender = "Amdad";
        tx.Receiver = "Topu";
        tx.Amount= 100;
        tx.TransactionTime=DateTime.Now;
        block.AddTransaction(tx);
        tx = new Transaction();
        tx.Sender = "Amdad";
        tx.Receiver = "BracCoin";
        tx.Amount = 1;
        tx.TransactionTime = DateTime.Now;
        block.AddTransaction(tx);
        Block minedBlock = Mine(block);
        string hash=bracCoin.CloseBlock(minedBlock);
        Console.WriteLine(hash);

        



    }
    public static Block Mine(Block block)
    {
        while (true)
        {
            string hash = HashGenerator.GenerateSHA256Hash(block);
            if (hash.StartsWith("00000"))
                return block;
            block.nonce++;
            Console.WriteLine(block.nonce);
        }
    }


}