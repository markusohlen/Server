using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class CreateInstructionStepModel
{
    [Required(ErrorMessage = "Instruction text is required.")]
    public string Text { get; set; } = string.Empty;
}
