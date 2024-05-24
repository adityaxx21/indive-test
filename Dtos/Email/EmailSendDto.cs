using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indive_test.Dtos.Email
{
    public class EmailSendDto
    {
        public required string  Email { get; set; }
        public string?  Subject { get; set; }
        public string?  Message { get; set; }

    }
}