using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmConnectFuncApp
{
    public class Order
    {
        string order_id = "order_id";
        string order_status = "order_status";
        }

    // Add enums for optionset fields
    enum OrderStatus
    {
        InProgress = 125080000,
        Complete = 125080001
      
    }
   
}
