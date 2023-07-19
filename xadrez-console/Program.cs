using System;
using tabuleiro;
using xadrez_console.tabuleiro;
using xadrez_console.xadrez;

namespace xadrez_console
{
    internal class Program {
        static void Main(string[] args) {
            try {
                PosicaoXadrez pos = new PosicaoXadrez('c', 7);
                Console.WriteLine(pos.ToString());
                Console.WriteLine(pos.toPosicao());
            } catch (TabuleiroException ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}