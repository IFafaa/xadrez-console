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

                tab.colocarPeca(new Torre(Cor.BRANCO, tab), new Posicao(0, 0));
                tab.colocarPeca(new Torre(Cor.PRETO, tab), new Posicao(0, 2));
                tab.colocarPeca(new Torre(Cor.BRANCO, tab), new Posicao(0, 4));
                tab.colocarPeca(new Rei(Cor.PRETO, tab), new Posicao(0, 5));

                Tela.imprimirTabuleiro(tab);
            } catch (TabuleiroException ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}