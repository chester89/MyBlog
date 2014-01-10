using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBlog.Core.Models;

namespace MyBlog.Core.Contracts
{
    public interface ICacheRepository: IStartable
    {
        TagModel[] Tags { get; set; }
        IDictionary<Int32, string[]> TagsByPost { get; set; }
    }
}
