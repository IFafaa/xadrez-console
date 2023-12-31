﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using xadrez_console.tabuleiro;

namespace tabuleiro {
    abstract class Peca {
        
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

        public void incrementarQntMovimentos() {
            qntMovimentos++;
        }

        public void decrementarQntMovimentos() {
            qntMovimentos--;
        }

        public bool existeMovimentosPossiveis() {
            bool[,] mat = movimentosPossiveis();
            for(int i = 0;  i < tab.linhas; i++) {
                for(int j = 0; j < tab.colunas; j++) {
                    if (mat[i, j]) {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool movimentoPossivel(Posicao pos) {
            return movimentosPossiveis()[pos.linha, pos.coluna];
        }
        public abstract bool[,] movimentosPossiveis();
        
    }
}
