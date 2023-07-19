using System;
using tabuleiro;
using xadrez_console.tabuleiro;
using xadrez_console.xadrez;

namespace xadrez_console
{
    internal class Program {
        static void Main(string[] args) {
            try {
                Tabuleiro tab = new Tabuleiro(8, 8);
                PartidaDeXadrez partida = new PartidaDeXadrez(tab);
                while (!partida.terminada) {
                    Console.Clear();
                    Tela.imprimirTabuleiro(partida.tab);
                    Console.Write("\nOrigem: ");
                    Posicao origem = Tela.lerPosicaoXadrez().toPosicao();
                    bool[,] posicoesPossiveis = partida.tab.peca(origem).movimentosPossiveis();

                    Console.Clear();
                    Tela.imprimirTabuleiro(partida.tab, posicoesPossiveis);
                    Console.Write("\nDestino: ");

                    Posicao destino = Tela.lerPosicaoXadrez().toPosicao();

                    partida.executaMovimento(origem, destino);
                }
            } catch (TabuleiroException ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}