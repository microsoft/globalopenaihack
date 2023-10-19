using Microsoft.SemanticKernel;

namespace CC.UI.Plugins.MedicalKnowledgePlugin
{
    public class MedicalKnowledgePlugin
    {
        private readonly IKernel _kernel;

        public MedicalKnowledgePlugin(IKernel kernel)
        {
            _kernel = kernel;
        }
    }
}
