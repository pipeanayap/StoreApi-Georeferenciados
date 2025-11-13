namespace StoreApi;

public class Prompts
{
    public static string GenerateOrdersPrompt(string jsonData)
    {
        return @$"
               Eres un analista experto de datos en retail.
                Analiza los siguientes datos de ordenes, productos y tiendas (en JSON)
                {jsonData}

                Debes responder **exclusivamente** en formato JSON y con esta estructura:
                {{
                    ""topProducts"": {{""name"":string, ""unitsSold"" : int, ""totalRevenue"":double}},
                    ""topStore"": {{""name"":string, ""totalSales"":double, ""sharedOfTtotalSales"": double}},
                    ""avgSpendeing"": double,
                    ""patterns"": [string]
                }}

                En el apartado de patterns, agrega analisis como: cual es la tienda que mas venden
                que productos son los que mas dinero dejan por orden.

                Si por alguna razon, no puedes generar esta respuesta valida (por ejemplo, te hace falta datos o tienes un error en el formato)            , 
                responde **SOLO** con el texto : error.
                No me saludes, no me des explicaciones, no me des comentarios y no incluyas texto adicional.
                    ";
    }

    public static string GenerateInvoicesPrompt(string jsonData)
    {
        return @$"
                Eres un analista experto de datos de facturas.
                Analiza los siguientes datos de facturas y ordenes (en JSON)
                {jsonData}

                Debes responder **exclusivamente** en formato JSON y con esta estructura:
                {{
                  ""totalInvoices"": int,
                  ""paidInvoices"": int,
                  ""unpaidInvoices"": int,
                  ""totalRevenue"": double,
                  ""averageInvoiceAmount"": double,
                  ""commonCurrencies"": [string],
                  ""patterns"": [string]
                }}

                En el apartado de patterns, agrega analisis como: 
                - Qué porcentaje de facturas están pagadas.
                - Qué moneda se usa más.
                - Cualquier patrón relevante detectado.

                Si por alguna razon, no puedes generar esta respuesta valida (por ejemplo, te hace falta datos o tienes un error en el formato)            , 
                responde **SOLO** con el texto : error.
                No me saludes, no me des explicaciones, no me des comentarios y no incluyas texto adicional.
                ";
    }
}