using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.CodeDom;

namespace Projeto_de_Cadastro_de_Usuários
{
    internal class Program
    {
        // Variáveis globais
        static string delimitadorInicio; // Delimitador de início da seção de dados no arquivo
        static string delimitadorFim; // Delimitador de fim da seção de dados no arquivo
        static string tagName; // Tag para o nome do usuário
        static string tagDataNascimento; // Tag para a data de nascimento do usuário
        static string tagNomeDaRua; // Tag para o nome da rua do usuário
        static string tagnumerodeCasa; // Tag para o número da casa do usuário
        static string tagnumeroDoDocumento; // Tag para o número do documento do usuário
        static string caminhoArquivo; // Caminho do arquivo de armazenamento dos dados


        public struct Dadoscadastrados // Estrutura para armazenar os dados de um usuário cadastrado
        {
            public string Nome;
            public string NomedaRua;
            public UInt32 numerodeCasa;
            public DateTime datadeNascimento;
            public string numeroDoDocumento;
        }
        public enum Resultdo_e // Enum para representar o resultado de uma operação
        {
            Sucesso = 0,
            Sair = 1,
            Excecao = 2
        }
        public static void MostrandoMensagem(String mensagem)// Função para mostrar uma mensagem no console e esperar por uma tecla 
        {
            Console.WriteLine(mensagem);
            Console.WriteLine("Precissione qualquer tecla para continuar");
            Console.ReadKey();
            Console.Clear();
        }
        public static Resultdo_e PegandoString(ref string minhaString, String mensagem) // Função para obter uma string digitada pelo usuário
        {
            Resultdo_e retorno;
            Console.WriteLine(mensagem);
            string temp = Console.ReadLine();
            if (temp == "s" || temp == "S")
            {
                retorno = Resultdo_e.Sair;
            }
            else
            {
                minhaString = temp;
                retorno = Resultdo_e.Sucesso;

            }
            Console.Clear();
            return retorno;
        }

        public static Resultdo_e PegandoData(ref DateTime data, string mensagem)// Função para obter uma data digitada pelo usuário
        {
            Resultdo_e retorno;

            do
            {
                try
                {
                    Console.WriteLine(mensagem);
                    string temp = Console.ReadLine();
                    if (temp == "s" || temp == "S")
                    {
                        retorno = Resultdo_e.Sair;
                    }
                    else
                    {
                        data = Convert.ToDateTime(temp);
                        retorno = Resultdo_e.Sucesso;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("EXCECAO: " + e.Message);
                    Console.WriteLine("Pressione qualquer tecla para continuar");
                    Console.ReadKey();
                    Console.Clear();
                    retorno = Resultdo_e.Excecao;
                }
            } while (retorno == Resultdo_e.Excecao);
            Console.Clear();
            return retorno;
        }
        public static Resultdo_e PegandoUInt32(ref UInt32 numero, string mensagem) // Função para obter um número inteiro digitado pelo usuário
        {
            Resultdo_e retorno;

            do
            {
                try
                {
                    Console.WriteLine(mensagem);
                    string temp = Console.ReadLine();
                    if (temp == "s" || temp == "S")
                    {
                        retorno = Resultdo_e.Sair;
                    }
                    else
                    {
                        numero = Convert.ToUInt32(temp);
                        retorno = Resultdo_e.Sucesso;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("EXCECAO: " + e.Message);
                    Console.WriteLine("Pressione qualquer tecla para continuar");
                    Console.ReadKey();
                    Console.Clear();
                    retorno = Resultdo_e.Excecao;
                }
            } while (retorno == Resultdo_e.Excecao);
            Console.Clear();
            return retorno;
        }

        public static void Listagem(ref List<Dadoscadastrados> listadeusuarios) // Cria uma lista em ordem de cadastado para mostrar o usuario cadastrados
        {
            foreach (Dadoscadastrados usuario in listadeusuarios)
            {
                Console.WriteLine($"Nome: {usuario.Nome}");
                Console.WriteLine($"Data de nascimento: {usuario.datadeNascimento}");
                Console.WriteLine($"Numero do documento: {usuario.numeroDoDocumento}");
                Console.WriteLine($"Nome da rua: {usuario.NomedaRua} - {usuario.numerodeCasa}");
                Console.WriteLine("-------------------------------------------");
            }
        }

        public static Resultdo_e Cadastrar(ref List<Dadoscadastrados> listadeusuarios)
        {
            Dadoscadastrados cadastrar;
            cadastrar.Nome = "";
            cadastrar.NomedaRua = "";
            cadastrar.datadeNascimento = new DateTime();
            cadastrar.numerodeCasa = 0;
            cadastrar.numeroDoDocumento = "";

            if (PegandoString(ref cadastrar.Nome, "Digite o nome completo ou digite S para sair") == Resultdo_e.Sair) return Resultdo_e.Sair;
            if (PegandoData(ref cadastrar.datadeNascimento, "Digite a data de nascimento no formato DD/MM/AAAA ou digite S para sair") == Resultdo_e.Sair) return Resultdo_e.Sair;
            if (PegandoString(ref cadastrar.numeroDoDocumento, "Digite o numero do documento ou digite S para sair") == Resultdo_e.Sair) return Resultdo_e.Sair;
            if (PegandoString(ref cadastrar.NomedaRua, "Digite o nome da rua ou digite S para sair") == Resultdo_e.Sair) return Resultdo_e.Sair;
            if (PegandoUInt32(ref cadastrar.numerodeCasa, "Digite o numero da casa ou digite S para sair") == Resultdo_e.Sair) return Resultdo_e.Sair;

            listadeusuarios.Add(cadastrar);
            GravaDados(caminhoArquivo, listadeusuarios);
            return Resultdo_e.Sucesso;
        }

        public static void GravaDados(string caminho, List<Dadoscadastrados> listadeusuarios)
        {
            try
            {
                string conteudoArquivo = "";
                foreach (Dadoscadastrados cadastro in listadeusuarios)
                {
                    conteudoArquivo += delimitadorInicio + "\r\n";
                    conteudoArquivo += tagName + cadastro.Nome + "\r\n";
                    conteudoArquivo += tagDataNascimento + cadastro.datadeNascimento.ToString("dd/MM/yyyy") + "\r\n";
                    conteudoArquivo += tagnumeroDoDocumento + cadastro.numeroDoDocumento + "\r\n";
                    conteudoArquivo += tagNomeDaRua + cadastro.NomedaRua + "\r\n";
                    conteudoArquivo += tagnumerodeCasa + cadastro.numerodeCasa + "\r\n";
                    conteudoArquivo += delimitadorFim + "\r\n";

                }
                File.WriteAllText(caminho, conteudoArquivo);
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCECAO: " + e.Message);
            }

        } //Gravando dados cadastrados
        public static void Carregadados(string caminho, ref List<Dadoscadastrados> listadeusuarios)
        {
            try
            {
                if (File.Exists(caminho))
                {
                    string[] conteudoArquivo = File.ReadAllLines(caminho);
                    Dadoscadastrados dadoscadastrados;
                    dadoscadastrados.Nome = "";
                    dadoscadastrados.datadeNascimento = new DateTime();
                    dadoscadastrados.NomedaRua = "";
                    dadoscadastrados.numerodeCasa = 0;
                    dadoscadastrados.numeroDoDocumento = "";


                    foreach (string linha in conteudoArquivo)
                    {
                        if (linha.Contains(delimitadorInicio)) continue;
                        if (linha.Contains(delimitadorFim))
                            listadeusuarios.Add(dadoscadastrados);
                        if (linha.Contains(tagName))
                            dadoscadastrados.Nome = linha.Replace(tagName, "");
                        if (linha.Contains(tagnumeroDoDocumento))
                            dadoscadastrados.numeroDoDocumento = linha.Replace(tagnumeroDoDocumento, "");
                        if (linha.Contains(tagDataNascimento))
                            dadoscadastrados.datadeNascimento = Convert.ToDateTime(linha.Replace(tagDataNascimento, ""));
                        if (linha.Contains(tagNomeDaRua))
                            dadoscadastrados.NomedaRua = linha.Replace(tagNomeDaRua, "");
                        if (linha.Contains(tagnumerodeCasa))
                            dadoscadastrados.numerodeCasa = Convert.ToUInt32(linha.Replace(tagnumerodeCasa, ""));
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("EXCECAO: " + e.Message);
            }
        }
        public static void BuscadordeUsuario(List<Dadoscadastrados> listadeusuarios)

        {
            Console.WriteLine("Digite o numero do documento para buscar o usuário ou digite S para sair");
            string temp = Console.ReadLine().ToLower();
            if (temp == "s")
            {
                return;
            }
            else
            {
                List<Dadoscadastrados> listadeusuariosTemp = listadeusuarios.Where(x => x.numeroDoDocumento == temp).ToList();
                if (listadeusuariosTemp.Count > 0)
                {
                    foreach (Dadoscadastrados usuario in listadeusuariosTemp)
                    {
                        Console.WriteLine(tagName + usuario.Nome);
                        Console.WriteLine(tagDataNascimento + usuario.datadeNascimento.ToString("dd/MM/yyyy"));
                        Console.WriteLine(tagnumeroDoDocumento + usuario.numeroDoDocumento);
                        Console.WriteLine(tagNomeDaRua + usuario.NomedaRua);
                        Console.WriteLine(tagnumerodeCasa + usuario.numerodeCasa);

                    }
                }
                else
                {
                    Console.WriteLine("Nenhum usuário possui o documento: " + temp);
                }

                MostrandoMensagem("");

            }
        } // Buscado de usuarios cadastrados

        public static void ExcluirUsuario(ref List<Dadoscadastrados> listadeusuarios)
        {
            Console.WriteLine("Digite o número do documento para excluir o usuário ou digite S para sair");
            string temp = Console.ReadLine().ToLower();
            bool alguemFoiExcluido = false;
            if (temp == "s")
                return;
            else
            {
                List<Dadoscadastrados> listadeusuariosTemp = listadeusuarios.Where(x => x.numeroDoDocumento == temp).ToList();
                if (listadeusuariosTemp.Count > 0)
                {
                    foreach (Dadoscadastrados usuario in listadeusuariosTemp)
                    {
                        listadeusuarios.Remove(usuario);
                        alguemFoiExcluido = true;
                    }
                    if (alguemFoiExcluido)
                    {
                        GravaDados(caminhoArquivo, listadeusuarios);
                        Console.WriteLine(listadeusuariosTemp.Count + "usuáro(s) com documento " + temp + "excluído(s)");
                    }
                }
                else
                {
                    Console.WriteLine("Nenhum usuario possui o documento: " + temp);
                }

            }
            MostrandoMensagem("");
        } // Vai excluir o usuario cadastrado

        static void Main(string[] args)
        {
            List<Dadoscadastrados> listadeusuarios = new List<Dadoscadastrados>();
            string op = "";
            delimitadorInicio = "##### INICIO #####";
            delimitadorFim = "##### FIM #####";
            tagDataNascimento = "DATA_DE_NASCIMENTO: ";
            tagNomeDaRua = "NOME_DA_RUA: ";
            tagnumerodeCasa = "NUMERO_DA_CASA: ";
            tagName = "NOME: ";
            tagnumeroDoDocumento = "NUMERO_DO_DOCUMENTO: ";
            caminhoArquivo = @"baseDeDadoos.txt";


            Carregadados(caminhoArquivo, ref listadeusuarios);


            do
            {
                Console.WriteLine("");
                Console.WriteLine("Digite C para cadastrar um novo usuario");
                Console.WriteLine("Digite B para buscar um usuário");
                Console.WriteLine("Digite E para excluir um usuário");
                Console.WriteLine("Digite L para ver a listagem de usuarios cadastrados ");
                Console.WriteLine("Digite S para sair");
                Console.WriteLine("");

                op = Console.ReadKey(true).KeyChar.ToString().ToLower();

                if (op == "c")
                {
                    //Cadastrando um novo usuario
                    Cadastrar(ref listadeusuarios);

                }
                else if (op == "b")
                {
                    //Buscar um usuario...
                    BuscadordeUsuario(listadeusuarios);

                }
                else if (op == "e")
                {

                    //Excluir um usuario...
                    ExcluirUsuario(ref listadeusuarios);

                }
                else if (op == "s")
                {
                    //Saindo do programa...
                    MostrandoMensagem("Encerrando o Programa");
                }
                else if (op == "l")
                {

                    Listagem(ref listadeusuarios);

                }
                else
                {
                    Console.WriteLine("Opção invalida");
                };


            } while (op != "s");
        }
    }
}