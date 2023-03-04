using MessagePipe;
using UnityEngine;
using VContainer;

/// <summary>
/// Generator�̐����𐧌䂷��R���|�[�l���g
/// </summary>
public class GenerateControl : MonoBehaviour
{
    int _quantity;

    [Inject]
    IPublisher<GeneratorControl> _publisher;

    public void CountUp()
    {
        _quantity++;

        if (_quantity >= ActorStatusUIManager.InstantiateMax)
        {
            _publisher.Publish(new GeneratorControl(isGeneratable: false));
        }
    }

    public void CountDown()
    {
        _quantity--;

        if (_quantity < ActorStatusUIManager.InstantiateMax)
        {
            _publisher.Publish(new GeneratorControl(isGeneratable: true));
        }
    }
}
