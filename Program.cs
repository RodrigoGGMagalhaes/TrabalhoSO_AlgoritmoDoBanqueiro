public class Banco // O banco será quem armazena e distribui os recursos
{
    private const int Numero_Clientes = 5; // Utilizando dos valores que foram providenciados através do enunciado
    private const int Numero_Recursos = 3; // Utilizando dos valores que foram providenciados através do enunciado

    private int[] RecursoDisponivel = new int[Numero_Recursos];
    private int[,] RecursoMaximo = new int[Numero_Clientes, Numero_Recursos];
    private int[,] RecursoAlocado = new int[Numero_Clientes, Numero_Recursos];
    private int[,] RecursoUtilizado = new int[Numero_Clientes, Numero_Recursos];

    private readonly object mutex = new object(); // Chave do bloqueio para a função lock (sem IA eu não ia saber nem que a função lock existia)

    private bool verificaStatus(int idCliente, int[] requisicao)
    {
        int[] DisponivelTemp = new int[Numero_Recursos];
        for (int i = 0; i < Numero_Recursos; i++)
            DisponivelTemp[i] = this.RecursoDisponivel[i];

        bool[] finalizados = new bool[Numero_Clientes];

        int[,] AlocadosTemp = new int[Numero_Clientes,Numero_Recursos];
        for (int i = 0; i < Numero_Clientes; i++)
            for (int j = 0; j < Numero_Recursos; j++)
                AlocadosTemp[i,j] = this.RecursoAlocado[i,j];

        int[,] UtilizadoTemp = new int[Numero_Clientes,Numero_Recursos];
        for (int i = 0; i < Numero_Clientes; i++)
            for (int j = 0; j < Numero_Recursos; j++)
                UtilizadoTemp[i,j] = this.RecursoUtilizado[i,j];
        
        for (int i = 0; i < Numero_Recursos; i++)
        {
            DisponivelTemp[i] -= requisicao[i];
            AlocadosTemp[idCliente, i] += requisicao[i];
            UtilizadoTemp[idCliente, i] -= requisicao[i];
        }

        // Algoritmo de segurança
    bool encontrou;

    do
    {
        encontrou = false;

        for (int cliente = 0; cliente < Numero_Clientes; cliente++)
        {
            if (!finalizados[cliente])
            {
                bool podeFinalizar = true;

                // Verifica se o cliente consegue terminar
                for (int recurso = 0; recurso < Numero_Recursos; recurso++)
                {
                    if (UtilizadoTemp[cliente, recurso] > DisponivelTemp[recurso])
                    {
                        podeFinalizar = false;
                        break;
                    }
                }

                // Se consegue terminar, devolve os recursos
                if (podeFinalizar)
                {
                    for (int recurso = 0; recurso < Numero_Recursos; recurso++)
                    {
                        DisponivelTemp[recurso] += AlocadosTemp[cliente, recurso];
                    }

                    finalizados[cliente] = true;
                    encontrou = true;
                }
            }
        }

    } while (encontrou);

    // Verifica se todos terminaram
    for (int i = 0; i < Numero_Clientes; i++)
    {
        if (!finalizados[i])
            return false;
    }

    return true;
    }

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

    public int SolicitaRecursos(int idCliente, int[] requisicao)
    {
        lock (mutex)
        {
            // Aqui é onde será verificado se o cliente ainda pode solicitar os recursos
            // E caso sim, será feito a atualização do vetor e matrizes para condizer com o valor
            for (int i = 0; i < Numero_Recursos; i++)
            {
                if (requisicao[i] > this.RecursoUtilizado[idCliente, i])
                    return -1;

                if (requisicao[i] > this.RecursoDisponivel[i])
                    return -1;

                if (!verificaStatus(idCliente, requisicao))
                    return -1;
            }

            for (int i = 0; i < Numero_Recursos; i++)
            {
                this.RecursoDisponivel[i] -= requisicao[i];
                this.RecursoAlocado[idCliente, i] += requisicao[i];
            }

            this.AtualizaDados();

            return 0;
        }
    }

    public int DevolveRecursos(int idCliente, int[] devolucao)
    {
        lock (mutex) 
        {
            // Essa função tem o objetivo de verificar e devolver um determinado numero de recursos
            for (int i = 0; i < Numero_Recursos; i++)
                if (this.RecursoAlocado[idCliente, i] - devolucao[i] < 0)
                    return -1;
        
            // Em seguida, é atualizado as tabelas para mostrar o número exato de recursos restantes
            for (int i = 0; i < Numero_Recursos; i++)
            {
                this.RecursoDisponivel[i] += devolucao[i];
                this.RecursoAlocado[idCliente, i] -= devolucao[i];
            }

            this.AtualizaDados();

            return 0;
        }
    }
}

class Program
{
    static void Main()
    {
        
    }
}