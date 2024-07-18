using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemFx : MonoBehaviour
{
    public void Explosion(Vector2 from, Vector2 to, float explo_range)
    {
        // ���� �̵�ȿ�� dotween �ҽ�
        // �Լ� ȣ�� �� �Էµ� ���ڸ� �� ��ũ��Ʈ�� ����� ������Ʈ ��ġ�� ���� 
        transform.position = from;

        // ������ �����, �����ϰ� ����� �������� �����
        Sequence sequence = DOTween.Sequence();

        // ������ Ʈ���� ������ ���� �߰�
        // ������Ʈ ������Ʈ�� ������ ������Ʈ�� �̵��� �����ð� ���� ��ȭ�Ѵ�.
        // (��ǥ��, ��ȭ�ð�), ��ȭ����(ť������)
        sequence.Append
            (transform.DOMove(from + Random.insideUnitCircle * explo_range, 0.25f).SetEase(Ease.OutCubic));
        sequence.Append
            (transform.DOMove(to, 0.5f).SetEase(Ease.InCubic));

        // �̸����� �Լ��� ȣ��Ǹ�
        sequence.AppendCallback(() =>
        {
            // �� ������Ʈ�� ����
            Destroy(gameObject);
        });
    }
}
