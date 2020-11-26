using HyperMarket.Queries.ApiManagers.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.ApiManagers
{
    public interface ITextMessageManager
    {
        Task SendMessage(TextMessageModel model);
    }
}
