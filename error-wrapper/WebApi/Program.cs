using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/weatherforecast", (WeatherForecast request) =>
{
    return ValidateItems(request)
        .Bind(_ => ValidateStock(request))
        .TryCatch(_=> ValidatePayment(), Error.Unauthorized)
        .Bind(transactionId => SubmitOrder(transactionId,request.Items))
        .Tap(_ => SendEmail())
        .Match(
            o => Results.Ok(o), 
            e => e.type switch
            {
                ErrorType.Validation => Results.BadRequest(e),
                ErrorType.Failure => Results.BadRequest(e),
                _ => Results.StatusCode(StatusCodes.Status500InternalServerError)
            });

});

static Result<WeatherForecast> ValidateItems(WeatherForecast request)
{
    return request.Items.Any() 
        ? Result<WeatherForecast>.Success(request) 
        : Result<WeatherForecast>.Failure(Error.NotLineItems);
}

static Result<WeatherForecast> ValidateStock(WeatherForecast request)
{
    return request.stock > 0 
        ? Result<WeatherForecast>.Success(request) 
        : Result<WeatherForecast>.Failure(Error.NotEnoughStock);
}

static string ValidatePayment()
{
    return Guid.NewGuid().ToString();
}

static Result<(string orderId, string transactionId)> SubmitOrder(string transactionId, List<string> items)
{
    var newOrderId = Guid.NewGuid().ToString();
    return Result<(string orderId, string transactionId)>.Success((newOrderId, transactionId));
}

static void SendEmail()
{
    // Send email logic
}


app.Run();




public static class ResultExtensions
{
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> bind)
    {
        return result.IsSuccess ? bind(result.Value) : Result<TOut>.Failure(result.Error);
    }

    public static Result<TOut> TryCatch<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> func, Error error)
    {
        try
        {
            return result.IsSuccess ? Result<TOut>.Success(func(result.Value)) 
                : Result<TOut>.Failure(result.Error); 
        }
        catch
        {
            return Result<TOut>.Failure(error);
        }   
    }

    public static Result<TIn> Tap<TIn>(this Result<TIn> result, Action<TIn> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value);
        }
        return result;
    }

    public static TOut Match<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
    }
}

internal abstract record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary,List<string> Items, int stock)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

