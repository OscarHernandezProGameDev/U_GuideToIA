public class GoHome : GAction
{
    public override bool PrePerform()
    {
        // lo ponemos aqui no en el post para evitar que use el baño
        beliefs.RemoveState("atHospital");
        return true;
    }

    public override bool PostPerform()
    {

        //Destroy(this.gameObject);
        return true;
    }
}
