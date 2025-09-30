using System.ComponentModel.DataAnnotations;

namespace MatriculasEAD.Models;

public class Aluno
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(120, ErrorMessage = "O nome deve ter no máximo 120 caracteres.")]
    [Display(Name = "Nome Completo")]
    public string Nome { get; set; } = null!;

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Digite um email válido.")]
    [StringLength(120, ErrorMessage = "O email deve ter no máximo 120 caracteres.")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = null!;

    [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres.")]
    [Phone(ErrorMessage = "Digite um telefone válido.")]
    [Display(Name = "Telefone")]
    public string? Telefone { get; set; }

    public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    [Display(Name = "Quantidade de Matrículas")]
    public int QuantidadeMatriculas => Matriculas.Count;
}