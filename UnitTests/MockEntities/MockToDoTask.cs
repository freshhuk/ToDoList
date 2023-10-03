using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.MockEntities
{
    internal class MockToDoTask
    {
        public int Id { get; set; }
        public string? NameTask { get; set; }
        public string? DescriptionTask { get; set; }
        public string? Status { get; set; }
        public DateTime TaskTime { get; set; }
    }
}
