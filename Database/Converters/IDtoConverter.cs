namespace UserService.Database.Converters
{
    public interface IDtoConverter<Model, Request, Response>
    {
        Model DtoToModel(Request request);
        Response ModelToDto(Model model);
    }
}
