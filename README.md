# Azure-AD-B2C

Passo a passo para implementação básica de utilização do Azure AD B2C com User Flows SignUp, SignIn e PasswordReset.

![image.png](/Images/banner-papo-de-dev-20220825.png)

[Assista a nossa Live do < Papo de Dev /> no Youtube](https://www.youtube.com/watch?v=v-3EbTlAoEo)

<a href="https://github.com/RLGHISLENI/Azure-AD-B2C/raw/master/Presentation/Papo-de-Dev_Azure-AD-B2C.pps" download>Faça o download da apresentação PowerPoint</a>

## Escopo

- Criação de Tenant;
- Criação e configuração de aplicação no portal;
- Criação e configuração de User Flow: Sign Up / Sign In / Reset Password;
- Testes no portal: Sign Up / Sign In / Reset Password;
- Instrumentação em aplicação .NET Core;

## O que você precisa para executar o Projeto

- [Microsoft® Azure Free Account - Get a $200 Free Credit](https://www.googleadservices.com/pagead/aclk?sa=L&ai=DChcSEwj1wuXGyef5AhVp6FwKHXEmD9IYABABGgJjZQ&ohost=www.google.com&cid=CAESaeD28f3B408dsUC9-zjWrXsYRtSSZ04yfs9qewIKFRHdn0j6Fts6jt-qidpAec2xEzYUtQnBHezD2XS_z2YfUmvwQHyspO0Sp43L5iqGeYc_jeEez49Rk6D3jMrOpRhA_LrTpiaybMn7HA&sig=AOD64_06cKaRog9mAKsBefr1U5PMc5ZjGA&q&adurl&ved=2ahUKEwjG89_Gyef5AhUnBLkGHU9PDX0Q0Qx6BAgDEAE)

- [Visual Studio 2022 Community](https://visualstudio.microsoft.com/pt-br/vs/community/)

## Criação do Tenant

Após realizar o **login em sua conta no Microsoft Azure**, adicione um novo recurso clicando no ícone demonstrado na imagem abaixo:

![image.png](/Images/01-create-a-resource.png)

Digite "**_b2c_**" na caixa de pesquisa e localize o recurso "**_Azure Active Directory B2C_**".

![image.png](/Images/02-create-a-resource-azure-active-directory-b2c.png)

Clique em "**_Create_**" para realizar a criação do recurso.

![image.png](/Images/03-create-azure-active-directory-b2c.png)

Escolha a opção "**Create a new Azure AD B2C Tenant**".

![image.png](/Images/04-create-b2c-tenant.png)

Selecione o tipo de Tenant conforme demonstrado abaixo:

![image.png](/Images/04-create-b2c-tenant-1.png)

Nesta mesma página, selecione a guia "**_Configuration_**" e vamos adicionar as informações do nosso diretório.

- Defina o nome da Organização;
- Defina o nome do Domínio;
- Crie um novo grupo de recursos (Agrupamento lógico de recursos); 

Clique em "**_Review + Create_**".

![image.png](/Images/04-create-b2c-tenant-2.png)

> **_Atenção:_** Para criação deste recurso é preciso possuir uma **_Subscription_** vinculada a sua conta do Azure mesmo que o recurso seja gratuíto.

Conclua a criação clicando em "**_Create_**".

![image.png](/Images/04-create-b2c-tenant-3.png)

 Aguarde a criação do "**_Resource Group_**" e do "**_Tenant_**.

![image.png](/Images/04-create-b2c-tenant-4.png)

Assim que a criação dos recursos forem concluídos, você pode **_clicar no link_** disponível na página e será redirecionado para o **_Tenant do Azure AD B2C_**.

![image.png](/Images/04-create-b2c-tenant-5.png)

## Configuração da aplicação no portal

Na caixa de pesquisa localizada no topo da página escreva "**_b2c_**", localize o recurso chamdo "**_Azure AD B2C_**" e clique nele e você será direcionado para página principal deste recurso.

![image.png](/Images/05-search-b2c.png)

Localize no menu lateral a opção "**_App Registrations_**" e clique nela.

![image.png](/Images/06-add-app-registration.png)

Dentro de **_App Registrations_**, registre um novo app clicando em "**_New registration_**".

![image.png](/Images/06-add-app-registration-1.png)

Adicione o _Name_, o _Account Type_ e a **_Redirect URI_** que é para onde o B2C deverá redirecionar o usuário após realização do **_Sign In_**.

Clique em **_Register_** para concluir.

![image.png](/Images/06-add-app-registration-2.png)

Na página do seu app criado, encontre no menu lateral a opção "**_Authentication_**" e clique nela.

![image.png](/Images/06-add-app-registration-3.png)

Adicione agora uma nova **_URI_** apontando para a aplicação desejada. Neste exemplo, adicionamos um redirecionamento para aplicação que irá rodar no localhost da máquina (_nossa aplicação .NET Core_).

```zsh
https://localhost:5001/signin-oidc
```

> Você poderá adicionar aqui o endereço de sua aplicação mas deve lembrar que esta URI sempre deverá terminar com "**_/signin-oidc_**".

Marque as duas caixas de seleção:

- Access tokens (used for implicit flows)
- ID tokens (used for implicit and hybrid flows)

Na parte inferior da página, ative a opção "**_Allow public client flows_**".

Conclua a operação clicando em "**_Save_**".

![image.png](/Images/06-add-app-registration-4.png)

![image.png](/Images/06-add-app-registration-5.png)

## Criação e configuração de User Flow

Voltando para página principal do Azure AD B2C, localize no menu lateral a opção "**_User Flows_**" e clique em "**_New user flow_**" para adicionarmos a jornada de usuário do nosso **_Sign Up & Sign In_**.

![image.png](/Images/06-add-user-flows.png)

Escolha o tipo de user flow "**_Sign Up and Sign In_**" e utilizando a versão recomendada clique em "**_Create_**".

![image.png](/Images/06-add-user-flows-1.png)

Defina o nome do nosso user flow e guarde esta informação, pois iremos utiliza-la mais adiante.

Marque as opções demonstradas nas imagens a seguir:

![image.png](/Images/06-add-user-flows-2.png)

Escolha a coleção de atributos e claims de retorno.

Clique em "**_Create_**" para concluir.

![image.png](/Images/06-add-user-flows-3.png)

Agora vamos adicionar na jornada de usuário o user flow de "**_Reset Password_**" do mesmo modo que acabamos de fazer, veja a imagem abaixo:

![image.png](/Images/06-add-user-flows-5.png)

Defina o nome desta user flow e guarde-o para utilizarmos mais adiante.

![image.png](/Images/06-add-user-flows-6.png)

Marque as claims que a user deverá retornar e clique no botão "**_Create_**" para concluir.

![image.png](/Images/06-add-user-flows-7.png)

Na página de "**_User Flows_**", clique no fluxo de **_Sign Up & Sign In_**.

![image.png](/Images/07-add-user-flows-signup-signin.png)

Localize e clique na a opção "**_Properties_**".

![image.png](/Images/07-add-user-flows-signup-signin-1.png)

Na seção "**_Password configuration_**" marque a opção "**_Self-service password reset_**" para permitir que o link de "_Forgot your password_" do fluxo de Sign Up & Sign In aponte para nossa user flow de "_**Reset Password**_" recem criada.

Clique em "**_Save_**" no topo da página para concluir a operação.

![image.png](/Images/07-add-user-flows-signup-signin-2.png)

Voltando a página da nossa user flow de Sign Up & Sign In, vamos alterar a configuração de idioma para utilizar Português Brasil.

Na seção "**_Customize_**", localize a opção "**_Languages_**" e clique nela.

![image.png](/Images/07-add-user-flows-signup-signin-3.png)

Clique na opção "**_Enable language customization_**" localizada no topo da página.

![image.png](/Images/07-add-user-flows-signup-signin-4.png)

Selecione o idioma "**_português (Brasil) pt-BR_**".
Abrirá uma janela no lado direito para habilitar a utilização deste idioma.

Marque a opção "**_Enabled_**" e clique em "**_Save_**" para aplicar.

![image.png](/Images/07-add-user-flows-signup-signin-5.png)

## Testes no portal

**Agora estamos prontos para testar nossa jornada de usuário completa via portal do Azure.**

Na página da nossa user flow de Sign Up & Sign In, localise o botão "**_Run user flow_**" e clique nele. 

![image.png](/Images/07-add-user-flows-signup-signin-6.png)

Abrirá uma janela no lado direito com as opções de execução da nossa user flow. 

Selecione "**_https://jwt.ms_**" na opção de "**_Reply URL_**" para podermos ver o token gerado após a conclusão do Sign Up & Sign In.

Clique em "**_Run user flow_**" para executar.

![image.png](/Images/07-add-user-flows-signup-signin-7.png)

Será apresentada a jornada de usuário de Sign Up & Sign In com Reset Password funcionando.

**_Bora testar..._**

![image.png](/Images/09-web-app-running-1.png)

Agora que nossas configurações e apontontamentos do Azure AD B2C estão protas, vamos criar uma "**_Secret_**" para podermos plugar nossa jornada de usuário ao nosso projeto .NET Core.

Volte a página de "App registrations" e acesse seu app criado.

![image.png](/Images/08-add-user-flows-app-registration.png)

 Localize no menu a opção "**_Certificaes & secrets_**" em seguida clique em "**_New client secret_**".

![image.png](/Images/08-add-user-flows-app-registration-1.png)

Será aberta uma janela do lado direito para configuração da "**_Secret_**".

Adicione a "**_Description_**" e "**_Expires_**" para a secret.

Clique no botão "**_Save_**" para concluir.

![image.png](/Images/08-add-user-flows-app-registration-2.png)

> IMPORTANTE: Após a criação da "**_Secret_**", copie o atributo "**_Value_**" dela e guarde em local seguro. 
>Esta informação não poderá mais ser resgatada  e em caso de perda, nova secret deverá ser criada.

Veja abaixo a secret recem criada com o valor exposto e uma já existente que não permite mais a visualização da informação.

![image.png](/Images/09-web-app-secret.png)

Agora, vamos obter as informações necessárias para configurarmos nosso projeto .NET Core.

Precisamos das seguintes informações:

- **ClientId** = Localizado na página de "**_App registrations_**".
![image.png](/Images/08-add-user-flows-app-registration-3.png)

- **Tenant** = Localizado na página do nosso app registrado.
![image.png](/Images/08-add-user-flows-app-registration-4.png)

- **SignUpSignInPolicyId** e **ResetPasswordPolicyId** = Nome das user flows criadas
![image.png](/Images/07-add-user-flows-signup-signin.png)

- **ClientSecret** = Secret que acabamos de criar (espero que você tenha guardado o valor, né!!!).

## Instrumentação da aplicação .NET Core

**Agora sim!!**

Abra o projeto no Visual Studio e adicione as informações no arquivo o ```appsettings.json```

![image.png](/Images/appsettings.png)
> IMPORTANTE: Deverá ser alterado o valor do texto "**_< TENANT >_**" pelo mesmo nome contido na chave Tenant acima, mas sem o domínio "**_.onmicrosoft.com_**".

Execute o projeto no Visual Studio e clique na opção login no canto superior direito da página.

![image.png](/Images/09-web-app-running.png)

Será direcionado para dentro do nosso Tenant e executada a jornada do usuário.

Realize as operações de Sign Up, Sign In e Reset Password para testar o funcionamento do projeto.

![image.png](/Images/09-web-app-running-1.png)

Quando a jornada do usuário for concluída o Azure AD B2C irá redirecionar para nossa aplicação com o Token válido e as claims definidas na configuração.

![image.png](/Images/09-web-app-logado.png)

Perceba que retornou a claim "**_DisplayName_**" contendo a informação que adicionei no momento do Sign Up.

## Referências

[Deploy custom policies devops](https://learn.microsoft.com/en-us/azure/active-directory-b2c/deploy-custom-policies-devops)

## Informações

![image.png](/Images/contatos.png)
