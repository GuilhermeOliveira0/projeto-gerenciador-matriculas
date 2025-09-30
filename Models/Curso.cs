using System.ComponentModel.DataAnnotations;

namespace MatriculasEAD.Models;

public class Curso
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(120, ErrorMessage = "O título deve ter no máximo 120 caracteres.")]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = null!;

    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    [Display(Name = "Descrição")]
    public string? Descricao { get; set; }

    [Range(0, 999999, ErrorMessage = "O preço base deve ser maior ou igual a zero.")]
    [Display(Name = "Preço Base")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal PrecoBase { get; set; }

    [Range(1, 9999, ErrorMessage = "A carga horária deve estar entre 1 e 9999 horas.")]
    [Display(Name = "Carga Horária (horas)")]
    public int CargaHoraria { get; set; }

    public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    [Display(Name = "Quantidade de Alunos")]
    public int QuantidadeAlunos => Matriculas.Count;
}