using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.DataTransferObjects
{
    public class SearchResultDTO
    {
        public string Id { get; set; }

        public string Topic { get; set; }

        public DateTime Date { get; set; }
    }
}
