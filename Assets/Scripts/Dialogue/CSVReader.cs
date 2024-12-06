using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader
{
    // 정규식: 쉼표로 데이터를 구분하되, 큰따옴표 내부의 쉼표는 무시
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";

    // 정규식: 다양한 줄바꿈 문자(\r\n, \n\r, \n, \r)를 처리
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    // 문자열의 앞뒤에서 제거할 문자 (주로 큰따옴표)
    static char[] TRIM_CHARS = { '\"' };

    /// <summary>
    /// CSV 파일을 읽어 첫 번째 열 값을 키로 하고 각 행 데이터를 Dictionary 형태로 반환.
    /// </summary>
    /// <param name="file">읽을 CSV 파일의 이름 (Resources 폴더 내 경로)</param>
    /// <returns>첫 번째 열 값을 키로 한 Dictionary</returns>
    public static Dictionary<string, Dictionary<string, string>> Read(string file)
    {
        Debug.Log("Read CSV: " + file);

        // 최종적으로 반환할 딕셔너리
        var result = new Dictionary<string, Dictionary<string, string>>();

        // Unity의 Resources.Load를 사용하여 파일 읽기
        TextAsset data = Resources.Load(file) as TextAsset;

        // 파일의 내용을 줄 단위로 분리
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        // 줄이 하나 이하일 경우 (헤더나 데이터가 없으면) 빈 딕셔너리 반환
        if (lines.Length <= 1) return result;

        // 첫 번째 줄을 헤더로 사용 (열 이름)
        var header = Regex.Split(lines[0], SPLIT_RE);

        // 데이터 시작 (1번째 줄부터) 각 행 처리
        for (var i = 1; i < lines.Length; i++)
        {
            // 현재 줄을 쉼표 기준으로 분리
            var values = Regex.Split(lines[i], SPLIT_RE);

            // 빈 줄은 건너뛴다
            if (values.Length == 0 || values[0] == "") continue;

            // 첫 번째 열 값 (Key)
            string key = values[0].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

            // 각 행 데이터를 담을 Dictionary 초기화
            var entry = new Dictionary<string, string>();

            // 열 이름(헤더)와 값 매칭
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                // 헤더(키)와 변환된 값(또는 원래 값)을 Dictionary에 추가
                entry[header[j]] = value;
            }

            // 첫 번째 열 값을 키로 사용하여 결과에 추가
            result[key] = entry;
        }

        // 모든 데이터를 처리한 후 딕셔너리 반환
        return result;
    }
}
