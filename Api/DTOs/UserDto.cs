using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
	public class UserDto
	{
		[Required]
		[StringLength(15, MinimumLength =3,ErrorMessage="Name must be between [2] and [16] characters")]
        public string Name { get; set; }

    }
}
