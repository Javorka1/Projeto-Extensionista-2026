using System;

namespace ProjetoExtensionista2026_2
{
    internal class rolesClass
    {
        public void RoleSidebar(formPrincipal formPrincipal, string role, string username, string passwordHash)
        {
            formPrincipal.listBoxSidebar.Items.Clear();

            string r = (role ?? "").ToLowerInvariant();


            formPrincipal.btnNavPrincipal.Visible = true;
            formPrincipal.btnNavConfig.Visible = true;
            formPrincipal.btnSair.Visible = true;

            if (r == "admin")
            {

                formPrincipal.btnNavVendas.Visible = true;
                formPrincipal.btnNavRegistroVendas.Visible = true;
                formPrincipal.btnNavEstoque.Visible = true;
                formPrincipal.btnNavClientes.Visible = true;
                formPrincipal.btnNavFinanceiro.Visible = true;

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

                formPrincipal.btnNavVendas.Visible = true;
                formPrincipal.btnNavRegistroVendas.Visible = true;
                formPrincipal.btnNavEstoque.Visible = false;
                formPrincipal.btnNavClientes.Visible = false;
                formPrincipal.btnNavFinanceiro.Visible = false;

                formPrincipal.listBoxSidebar.Items.Add("Principal");
                formPrincipal.listBoxSidebar.Items.Add("Vendas");
                formPrincipal.listBoxSidebar.Items.Add("Configurações");
                formPrincipal.listBoxSidebar.Items.Add("Sair");
            }
            else if (r == "stocker")
            {

                formPrincipal.btnNavVendas.Visible = false;
                formPrincipal.btnNavRegistroVendas.Visible = false;
                formPrincipal.btnNavEstoque.Visible = true;
                formPrincipal.btnNavClientes.Visible = false;
                formPrincipal.btnNavFinanceiro.Visible = false;

                formPrincipal.listBoxSidebar.Items.Add("Principal");
                formPrincipal.listBoxSidebar.Items.Add("Estoque");
                formPrincipal.listBoxSidebar.Items.Add("Configurações");
                formPrincipal.listBoxSidebar.Items.Add("Sair");
            }
            else if (r == "monetary")
            {

                formPrincipal.btnNavVendas.Visible = false;
                formPrincipal.btnNavRegistroVendas.Visible = true;
                formPrincipal.btnNavEstoque.Visible = false;
                formPrincipal.btnNavClientes.Visible = true;
                formPrincipal.btnNavFinanceiro.Visible = false;

                formPrincipal.listBoxSidebar.Items.Add("Principal");
                formPrincipal.listBoxSidebar.Items.Add("Financeiro");
                formPrincipal.listBoxSidebar.Items.Add("Configurações");
                formPrincipal.listBoxSidebar.Items.Add("Sair");
            }
            else
            {

                formPrincipal.btnNavVendas.Visible = false;
                formPrincipal.btnNavRegistroVendas.Visible = false;
                formPrincipal.btnNavEstoque.Visible = false;
                formPrincipal.btnNavClientes.Visible = false;
                formPrincipal.btnNavFinanceiro.Visible = false;
                formPrincipal.listBoxSidebar.Items.Add("Sair");
            }
        }
    }
}
