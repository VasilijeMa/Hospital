// See https://aka.ms/new-console-template for more information

using ZdravoCorp.Core.PhysicalAssets.Services;
using ZdravoCorpCLI;

Timer _renovator = new Timer(Renovate, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

static void Renovate(object state)
{
    RenovationExecutionService renovationService = new RenovationExecutionService();
    renovationService.UpdateRenovations();
}

MainCLIView mainCLIView = new MainCLIView();