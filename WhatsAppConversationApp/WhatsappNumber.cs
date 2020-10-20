using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using WhatsAppConversationApp.Enums;

namespace WhatsAppConversationApp
{
    public class WhatsappNumber
    {
        public NationalCode nationalCode;
        private string numberWithoutCC;
        private string _number;

        public string number
        {
            get
            {
                //return (int)nationalCode + " " + numberWithoutCC;
                return _number;
            }
            set
            {
                nationalCode = NationalCode.Pakistan;
                numberWithoutCC = value;
                _number = value;
            }
        }

        //public WhatsappNumber()
        //{

        //}

        public WhatsappNumber(string Number)
            //: this()
        {
            _number = Number;
            Regex r = new Regex(@"^\+([1-9]{1,3})\s(\d{1,14})$");
            Match match = r.Match(Number);
            //nationalCode = (NationalCode)(Int32.Parse(match.Groups[1].Value));
            //numberWithoutCC = Number;
        }

        public WhatsappNumber(NationalCode country_code, string Number)
            : this(Number)
        {
            nationalCode = country_code;
        }
    }
}
