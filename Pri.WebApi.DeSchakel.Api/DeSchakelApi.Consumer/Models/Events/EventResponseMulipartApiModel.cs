using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Models.Events
{
    public class EventResponseMulipartApiModel
    {
        public List<StringContent> ProgrammatorIds { get; set; }
        public List<StringContent> GenreIds { get; set; }
        public StreamContent Image { get; set; }
        public StringContent Id { get; set; }
        public StringContent Title {  get; set; }
        public StringContent EventDate { get; set; }
        public StringContent Description { get; set; }
        public StringContent CompanyId { get; set; }
        public StringContent LocationId { get; set; }
        public StringContent SuccesRate {  get; set; }
       public StringContent Price {  get; set; }
    }
}
