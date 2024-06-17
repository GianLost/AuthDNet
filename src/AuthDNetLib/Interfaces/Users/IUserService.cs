namespace AuthDNetLib.Interfaces.Users;

/// <summary>
/// Define o contrato para operações CRUD básicas (Criar, Ler, Atualizar, Deletar) para uma entidade de usuário genérica.
/// </summary>
/// <typeparam name="T">O tipo de entidade de usuário a ser gerenciado. Deve ser uma classe.</typeparam>
public interface IUserService<T> where T : class
{
    /// <summary>
    /// Cria um novo usuário no banco de dados, criptografando as propriedades sensíveis (como senha e token de autenticação) antes de salvar as alterações.
    /// </summary>
    /// <param name="user">O objeto de usuário a ser criado.</param>
    /// <returns>O objeto de usuário criado, com as propriedades criptografadas, se aplicável.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando ocorre um erro durante a criação do usuário ou ao salvar as alterações no banco de dados.
    /// </exception>
    Task<T> CreateUserAsync(T user);

    /// <summary>
    /// Atualiza os detalhes de um usuário existente no banco de dados.
    /// </summary>
    /// <param name="user">A entidade de usuário a ser atualizada.</param>
    /// <returns>O objeto de usuário atualizado.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando ocorre um erro durante a atualização do usuário ou ao salvar as alterações no banco de dados.
    /// </exception>
    Task<T> UpdateUserAsync(T user);

    /// <summary>
    /// Obtém uma coleção de todos os usuários presentes no banco de dados.
    /// </summary>
    /// <returns>Uma lista de entidades de usuário.</returns>
    Task<ICollection<T>> GetUsersAsync();

    /// <summary>
    /// Exclui um usuário do banco de dados com base no ID especificado.
    /// </summary>
    /// <param name="id">O ID do usuário a ser excluído.</param>
    /// <returns>O objeto de usuário excluído.</returns>
    /// <exception cref="ArgumentNullException">Lançada quando o usuário com o ID especificado não é encontrado.</exception>
    Task<T> DeleteUserAsync(string id);

    /// <summary>
    /// Obtém um usuário específico do banco de dados com base no ID especificado.
    /// </summary>
    /// <param name="id">O ID do usuário a ser recuperado.</param>
    /// <returns>O objeto de usuário correspondente ao ID fornecido.</returns>
    /// <exception cref="ArgumentNullException">Lançada quando o usuário com o ID especificado não é encontrado.</exception>
    Task<T> GetUserByIdAsync(string id);
}