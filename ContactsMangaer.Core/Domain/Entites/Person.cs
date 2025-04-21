using System;
using System.ComponentModel.DataAnnotations;
namespace Enities
{
    /// <summary>
    /// its a domain model class
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        [StringLength(40)] //nvarchar(40)
        public string? PersonName { get; set; }

        [StringLength(40)] //nvarchar(40)
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)] //nvarchar(40)
        public string? Gender { get; set; }

        //uniqueidentifier
        public Guid? CountryID { get; set; }

        [StringLength(200)] //nvarchar(40)
        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }

        public Country? Country { get; set; }

    }
}
