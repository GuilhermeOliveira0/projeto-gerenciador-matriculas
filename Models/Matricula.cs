using System.ComponentModel.DataAnnotations;

namespace MatriculasEAD.Models;

public class Matricula : IValidatableObject
{
    public int AlunoId { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.ForeignKey("AlunoId")]
    public Aluno? Aluno { get; set; }

    public int CursoId { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.ForeignKey("CursoId")]
    public Curso? Curso { get; set; }

    [Display(Name = "Data da Matrícula")]
    public DateTime Data { get; set; } = DateTime.UtcNow;

    [Display(Name = "Preço Pago")]
    [Range(0, 999999, ErrorMessage = "O preço deve ser maior ou igual a zero.")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal PrecoPago { get; set; }

    [Display(Name = "Status")]
    public StatusMatricula Status { get; set; } = StatusMatricula.Ativa;

    [Display(Name = "Progresso (%)")]
    [Range(0, 100, ErrorMessage = "O progresso deve estar entre 0 e 100.")]
    public int Progresso { get; set; }

    [Display(Name = "Nota Final")]
    [Range(0, 10, ErrorMessage = "A nota final deve estar entre 0 e 10.")]
    public decimal? NotaFinal { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Regra: Para marcar como Concluído, o progresso deve ser 100%
        if (Status == StatusMatricula.Concluida && Progresso != 100)
        {
            yield return new ValidationResult(
                "Para alterar o status para 'Concluída', o progresso deve ser 100%.",
                new[] { nameof(Status) });
        }

        // Regra: Nota final só pode ser definida se o progresso for 100% (apenas para novos registros com nota)
        if (NotaFinal.HasValue && NotaFinal.Value > 0 && Progresso < 100)
        {
            yield return new ValidationResult(
                "A nota final só pode ser definida quando o progresso for 100%.",
                new[] { nameof(NotaFinal) });
        }
    }
}