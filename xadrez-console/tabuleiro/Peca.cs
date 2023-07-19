using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using xadrez_console.tabuleiro;

namespace tabuleiro {
    internal class Peca {
        
        public Posicao posicao { get; set; }
        public Cor cor { get; protected set; }
        public int qntMovimentos { get; protected set; }
        public Tabuleiro tab { get; protected set; }
        public Peca() {
        }
        public Peca(Cor cor, Tabuleiro tab) {
            this.posicao = null;
            this.cor = cor;
            this.tab = tab;
            qntMovimentos = 0;
        }
        
    }
}
