using System;
using System.Collections.Generic;

namespace GasPrice.Core.ViewModels
{
    public class FileResult
    {
        public IEnumerable<string> FileNames { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string DownloadLink { get; set; }
        public IEnumerable<string> ContentTypes { get; set; }
        public IEnumerable<string> Names { get; set; }
    }
}
