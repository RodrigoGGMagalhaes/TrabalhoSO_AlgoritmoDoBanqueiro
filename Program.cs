public class Banco // O banco será quem armazena e distribui os recursos
{
    private const int Numero_Clientes = 5;
    private const int Numero_Recursos = 3; // Utilizando dos valores que foram providenciados através do enunciado

    private int[] RecursoDisponivel = new int[Numero_Recursos];
    private int[,] RecursoMaximo = new int[Numero_Clientes, Numero_Recursos];
    private int[,] RecursoAlocado = new int[Numero_Clientes, Numero_Recursos];
    private int[,] RecursoUtilizado = new int[Numero_Clientes, Numero_Recursos];

    public void DefineDisponivel(int[] Recursos)
    {
        // Define a quantidade disponível de cada recurso no sistema
        for (int i = 0; i < this.RecursoDisponivel.Length; i++)
            this.RecursoDisponivel[i] = Recursos[i];
    }

    public void DefineMaximo(int idCliente, int[] MaximoRecursos)
    {
        // Define o quanto cada cliente ainda pode pedir de cada recurso
        // Para isso é necessário o idCliente para providenciar o valor correto
        for (int i = 0; i < this.RecursoMaximo.GetLength(1); i++)
            this.RecursoMaximo[idCliente, i] = MaximoRecursos[i];
    }

    public void AtualizaDados()
    {
        // Atualiza o vetor de utilizados conforme o que foi alocado para cada cliente
        for (int i = 0; i < Numero_Clientes; i++)
            for (int j = 0; j < Numero_Recursos; j++)
                this.RecursoUtilizado[i, j] = RecursoMaximo[i, j] - RecursoAlocado[i, j];
    }

    public bool SolicitaRecursos(int idCliente, int[] requisicao)
    {
        // Aqui é onde será verificado se o cliente ainda pode solicitar os recursos
        // E caso sim, será feito a atualização do vetor e matrizes para condizer com o valor
        for (int i = 0; i < Numero_Recursos; i++)
        {
            if (requisicao[i] > this.RecursoUtilizado[idCliente, i])
                return false;

            if (requisicao[i] > this.RecursoDisponivel[i])
                return false;
        }

        for (int i = 0; i < Numero_Recursos; i++)
        {
            this.RecursoDisponivel[i] -= requisicao[i];
            this.RecursoAlocado[idCliente, i] += requisicao[i];
        }

        this.AtualizaDados();

        return true;
    }

    public void ReleaseResources(int idCliente, int[] devolucao)
    {
        // Essa função tem o objetivo de devolver um determinado numero de recursos
        // Em seguida, é atualizado as tabelas para mostrar o número exato de recursos restantes
        for (int i = 0; i < Numero_Recursos; i++)
        {
            this.RecursoDisponivel[i] += devolucao[i];
            this.RecursoAlocado[idCliente, i] -= devolucao[i];
        }

        this.AtualizaDados();
    }
}

class Program
{
    static void Main()
    {
        
    }
}