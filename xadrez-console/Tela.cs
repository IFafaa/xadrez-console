using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using xadrez_console.tabuleiro;
using xadrez_console.xadrez;

namespace xadrez_console {
    internal class Tela {
        public static void imprimirTabuleiro(Tabuleiro tab) {
            for (int i = 0; i < tab.linhas;  i++) {
                Console.Write(8 - i + " ");
                for(int j = 0; j < tab.colunas; j++) {
                    Posicao pos = new Posicao(i, j);
                    imprimirPeca(tab.peca(pos));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void imprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPossiveis) {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;
            for (int i = 0; i < tab.linhas; i++) {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.colunas; j++) {
                    Posicao pos = new Posicao(i, j);
                    if (posicoesPossiveis[pos.linha, pos.coluna]) {
                        Console.BackgroundColor = fundoAlterado;
                    }
                    else {
                        Console.BackgroundColor= fundoOriginal;
                    }
                    imprimirPeca(tab.peca(pos));
                    Console.BackgroundColor = fundoOriginal;

                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");

        }

        public static void imprimirPeca(Peca peca) {
            if(peca == null) {
                Console.Write("- ");
            }
            else {
                if (peca.cor == Cor.BRANCO) {
                    Console.Write(peca);
                }
                else {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }

        public static PosicaoXadrez lerPosicaoXadrez() {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new PosicaoXadrez(coluna, linha);
        }
    }
}
