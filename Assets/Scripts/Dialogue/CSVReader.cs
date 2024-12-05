using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader
{
    // ���Խ�: ��ǥ�� �����͸� �����ϵ�, ū����ǥ ������ ��ǥ�� ����
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";

    // ���Խ�: �پ��� �ٹٲ� ����(\r\n, \n\r, \n, \r)�� ó��
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    // ���ڿ��� �յڿ��� ������ ���� (�ַ� ū����ǥ)
    static char[] TRIM_CHARS = { '\"' };

    /// <summary>
    /// CSV ������ �о� ù ��° �� ���� Ű�� �ϰ� �� �� �����͸� Dictionary ���·� ��ȯ.
    /// </summary>
    /// <param name="file">���� CSV ������ �̸� (Resources ���� �� ���)</param>
    /// <returns>ù ��° �� ���� Ű�� �� Dictionary</returns>
    public static Dictionary<string, Dictionary<string, string>> Read(string file)
    {
        Debug.Log("Read CSV: " + file);

        // ���������� ��ȯ�� ��ųʸ�
        var result = new Dictionary<string, Dictionary<string, string>>();

        // Unity�� Resources.Load�� ����Ͽ� ���� �б�
        TextAsset data = Resources.Load(file) as TextAsset;

        // ������ ������ �� ������ �и�
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        // ���� �ϳ� ������ ��� (����� �����Ͱ� ������) �� ��ųʸ� ��ȯ
        if (lines.Length <= 1) return result;

        // ù ��° ���� ����� ��� (�� �̸�)
        var header = Regex.Split(lines[0], SPLIT_RE);

        // ������ ���� (1��° �ٺ���) �� �� ó��
        for (var i = 1; i < lines.Length; i++)
        {
            // ���� ���� ��ǥ �������� �и�
            var values = Regex.Split(lines[i], SPLIT_RE);

            // �� ���� �ǳʶڴ�
            if (values.Length == 0 || values[0] == "") continue;

            // ù ��° �� �� (Key)
            string key = values[0].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

            // �� �� �����͸� ���� Dictionary �ʱ�ȭ
            var entry = new Dictionary<string, string>();

            // �� �̸�(���)�� �� ��Ī
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                // ���(Ű)�� ��ȯ�� ��(�Ǵ� ���� ��)�� Dictionary�� �߰�
                entry[header[j]] = value;
            }

            // ù ��° �� ���� Ű�� ����Ͽ� ����� �߰�
            result[key] = entry;
        }

        // ��� �����͸� ó���� �� ��ųʸ� ��ȯ
        return result;
    }
}
