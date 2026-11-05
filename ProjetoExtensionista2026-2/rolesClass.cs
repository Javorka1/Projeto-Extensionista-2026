using System;

namespace ProjetoExtensionista2026_2
{
    internal class rolesClass
    {
        public void RoleSidebar(formPrincipal formPrincipal, string role, string username, string passwordHash)
        {
            formPrincipal.listBoxSidebar.Items.Clear();

            string r = (role ?? "").ToLowerInvariant();

            // Configuração dos botões modernos da sidebar
            formPrincipal.btnNavPrincipal.Visible = true;
            formPrincipal.btnNavConfig.Visible = true;
            formPrincipal.btnSair.Visible = true;

            if (r == "admin")
            {
                // Administrador tem acesso completo
                formPrincipal.btnNavVendas.Visible = true;
                formPrincipal.btnNavEstoque.Visible = true;
                formPrincipal.btnNavFinanceiro.Visible = true;
                formPrincipal.btnNavCadastro.Visible = true;

                formPrincipal.listBoxSidebar.Items.Add("Principal");
                formPrincipal.listBoxSidebar.Items.Add("Cadastro");
                formPrincipal.listBoxSidebar.Items.Add("Vendas");
                formPrincipal.listBoxSidebar.Items.Add("Estoque");
                formPrincipal.listBoxSidebar.Items.Add("Financeiro");
                formPrincipal.listBoxSidebar.Items.Add("Configurações");
                formPrincipal.listBoxSidebar.Items.Add("Sair");
            }
            else if (r == "seller")
            {
                // Vendedor acessa Vendas e Principal
                formPrincipal.btnNavVendas.Visible = true;
                formPrincipal.btnNavEstoque.Visible = false;
                formPrincipal.btnNavFinanceiro.Visible = false;
                formPrincipal.btnNavCadastro.Visible = false;

                formPrincipal.listBoxSidebar.Items.Add("Principal");
                formPrincipal.listBoxSidebar.Items.Add("Vendas");
                formPrincipal.listBoxSidebar.Items.Add("Configurações");
                formPrincipal.listBoxSidebar.Items.Add("Sair");
            }
            else if (r == "stocker")
            {
                // Estoquista acessa Estoque e Principal
                formPrincipal.btnNavVendas.Visible = false;
                formPrincipal.btnNavEstoque.Visible = true;
                formPrincipal.btnNavFinanceiro.Visible = false;
                formPrincipal.btnNavCadastro.Visible = false;

                formPrincipal.listBoxSidebar.Items.Add("Principal");
                formPrincipal.listBoxSidebar.Items.Add("Estoque");
                formPrincipal.listBoxSidebar.Items.Add("Configurações");
                formPrincipal.listBoxSidebar.Items.Add("Sair");
            }
            else if (r == "monetary")
            {
                // Financeiro acessa Financeiro e Principal
                formPrincipal.btnNavVendas.Visible = false;
                formPrincipal.btnNavEstoque.Visible = false;
                formPrincipal.btnNavFinanceiro.Visible = true;
                formPrincipal.btnNavCadastro.Visible = false;

                formPrincipal.listBoxSidebar.Items.Add("Principal");
                formPrincipal.listBoxSidebar.Items.Add("Financeiro");
                formPrincipal.listBoxSidebar.Items.Add("Configurações");
                formPrincipal.listBoxSidebar.Items.Add("Sair");
            }
            else
            {
                // Acesso restrito
                formPrincipal.btnNavVendas.Visible = false;
                formPrincipal.btnNavEstoque.Visible = false;
                formPrincipal.btnNavFinanceiro.Visible = false;
                formPrincipal.btnNavCadastro.Visible = false;
                formPrincipal.listBoxSidebar.Items.Add("Sair");
            }
        }
    }
}
