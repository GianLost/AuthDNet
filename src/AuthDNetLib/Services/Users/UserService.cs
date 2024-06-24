using AuthDNetLib.Data;
using AuthDNetLib.Interfaces.Users;
using Microsoft.EntityFrameworkCore;

namespace AuthDNetLib.Services.Users;

/// <summary>
/// Implementação do serviço de usuário que oferece operações CRUD básicas (Criar, Ler, Atualizar, Deletar) para uma entidade de usuário genérica.
/// </summary>
/// <typeparam name="T">O tipo de entidade de usuário a ser gerenciado. Deve ser uma classe.</typeparam>
/// <remarks>
/// Inicializa uma nova instância de <see cref="UserService{T}"/>.
/// </remarks>
/// <param name="database">O contexto do banco de dados a ser usado para manipular os dados da entidade.</param>
public class UserService<T>(ApplicationDbContext database) : IUserService<T> where T : class
{
    /// <summary>
    /// Contexto do banco de dados utilizado para acessar e manipular os dados da entidade.
    /// </summary>
    private readonly ApplicationDbContext _database = database ?? throw new ArgumentNullException(nameof(database));

    /// <summary>
    /// Cria um novo usuário no banco de dados e salva as alterações.
    /// </summary>
    /// <param name="user">A entidade de usuário a ser criada.</param>
    /// <returns>O usuário criado com senha e token criptografados.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando ocorre um erro ao criar o usuário ou ao salvar as alterações no banco de dados.
    /// </exception>
    public virtual async Task<T> CreateUserAsync(T user)
    {
        try
        {
            await _database.Set<T>().AddAsync(user); // Adiciona o usuário na tabela do banco de daos
            await _database.SaveChangesAsync(); // Salva as mudanças no banco de dados.

            return user;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Erro ao salvar usuário no banco de dados.", ex);
        }
    }

    /// <summary>
    /// Atualiza um usuário existente no banco de dados.
    /// </summary>
    /// <param name="user">A entidade de usuário a ser atualizada.</param>
    /// <returns>O usuário atualizado.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando ocorre um erro ao atualizar o usuário ou ao salvar as alterações no banco de dados.
    /// </exception>
    public virtual async Task<T> UpdateUserAsync(T user)
    {
        try
        {
            _database.Set<T>().Update(user);
            await _database.SaveChangesAsync();

            return user;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Erro ao atualizar usuário no banco de dados.", ex);
        }
    }

    /// <summary>
    /// Obtém uma lista de todos os usuários presentes no banco de dados.
    /// </summary>
    /// <returns>Uma lista de usuários presentes no banco de dados.</returns>
    public virtual async Task<ICollection<T>> GetUsersAsync()
    {
        return await _database.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Exclui um usuário do banco de dados com base no ID especificado.
    /// </summary>
    /// <param name="id">O ID do usuário a ser excluído.</param>
    /// <returns>O usuário excluído.</returns>
    /// <exception cref="ArgumentNullException">
    /// Lançada quando o usuário com o ID especificado não é encontrado.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando ocorre um erro ao excluir o usuário ou ao salvar as alterações no banco de dados.
    /// </exception>
    public virtual async Task<T> DeleteUserAsync(string id)
    {
        try
        {
            T? user = await _database.Set<T>().FindAsync(id) ?? throw new ArgumentNullException(nameof(id), "Usuário não encontrado.");

            _database.Set<T>().Remove(user);
            await _database.SaveChangesAsync();

            return user;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Erro ao excluir usuário no banco de dados.", ex);
        }
    }

    /// <summary>
    /// Obtém um usuário específico do banco de dados com base no ID especificado.
    /// </summary>
    /// <param name="id">O ID do usuário a ser recuperado.</param>
    /// <returns>O usuário correspondente ao ID fornecido.</returns>
    /// <exception cref="ArgumentNullException">
    /// Lançada quando o usuário com o ID especificado não é encontrado.
    /// </exception>
    public virtual async Task<T> GetUserByIdAsync(string id)
    {
        T? user = await _database.Set<T>().FindAsync(id) ?? throw new ArgumentNullException(nameof(id), "Usuário não encontrado.");
        return user;
    }
}