using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Parallel_Programming_Hw_2
{
    public class Menager
    {
        object locker = new object();

        public Dictionary<Product, int> Stock { get; set; }

        public Menager(Dictionary<Product, int> stock)
        {
            Stock = stock;
        }

        public void Supply(Dictionary<Product, int> itemsToSupply)
        {
            lock (locker)
            {
                foreach (var itemToSupply in itemsToSupply)
                {
                    if (Stock.ContainsKey(itemToSupply.Key))
                    {
                        Stock[itemToSupply.Key] += itemToSupply.Value;
                    }
                    else
                    {
                        Stock.Add(itemToSupply.Key, itemToSupply.Value);
                    }
                }
                Console.WriteLine("Supply successful");
            }
        }

        public ResultEnum Purchase(Client client, Dictionary<Product, int> itemsToPurchase)
        {
            lock (locker)
            {

                bool isOk = false;
                foreach (var itemToPurchase in itemsToPurchase)
                {
                    if (Stock.ContainsKey(itemToPurchase.Key) 
                        && Stock[itemToPurchase.Key] >= itemToPurchase.Value) isOk = true;
                    else 
                    {
                        isOk = false;
                        break;
                    }
                }

                if (isOk)
                {
                    foreach (var itemToPurchase in itemsToPurchase)
                    {
                        if (Stock.ContainsKey(itemToPurchase.Key)
                            && Stock[itemToPurchase.Key] >= itemToPurchase.Value)
                        {
                            Stock[itemToPurchase.Key] -= itemToPurchase.Value;
                            //Console.WriteLine("Purchase successful");
                            Console.WriteLine(client.Name + " purchased successfully " + itemToPurchase.Key.Name);
                            return ResultEnum.Successful;
                        }
                        else
                        { 
                            Console.WriteLine("Purchase failed");
                            return ResultEnum.Failed;
                        }
                       
                    }
                }
                return ResultEnum.Failed;
            }
        }

        public Product GetProductByName(string name) 
        {
            Product result = Stock.Where(t => t.Key.Name.Equals(name)).FirstOrDefault().Key;

            return result;
        }
    }
}
