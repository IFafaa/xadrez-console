using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using xadrez_console.tabuleiro;

namespace xadrez_console {
    internal class Tela {
        public static void imprimirTabuleiro(Tabuleiro tab) {
            for (int i = 0; i < tab.linhas;  i++) {
                for(int j = 0; j < tab.colunas; j++) {
                    Posicao pos = new Posicao(i, j);
                    if(tab.peca(pos) == null) 
                        Console.Write("- ");
                    else 
                        Console.Write($"{tab.peca(pos).ToString()} ");
                }
                Console.WriteLine();
            }
        }
    }
}
