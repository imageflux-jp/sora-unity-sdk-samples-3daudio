using UnityEditor;
using UnityEngine;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;


public class GetStatsWindow : EditorWindow
{
    private StringBuilder _stringBuilder = new StringBuilder();

    private string[] whitelist = new string[] { "GetStats:" };

    private List<float> data_ = new List<float>();

    private InputJson inputJson;


    // メニューからウィンドウを表示する
    [MenuItem("Sora/GetStats/StatsWindow")]
    public static void OpenWindow()
    {
        GetStatsWindow.GetWindow(typeof(GetStatsWindow));
    }

    private void OnEnable()
    {
        // コンソールのログを取得するようにする
        Application.logMessageReceived += OnReceiveLog;
    }

    private void OnReceiveLog(string logText, string stackTrace, LogType logType)
    {

        _stringBuilder.Clear();

        if (0 < whitelist.Length)
        {
            // Getstats 以外は不要なので除外
            for (int i = 0; i < whitelist.Length; i++)
            {
                if (whitelist[i] == string.Empty)
                {
                    return;
                }
                else if (!logText.Contains(whitelist[i]))
                {
                    return;
                }
            }
        }
        _stringBuilder.Append(logText);
    }

    // GetStats のラインナップ
    [Serializable]
    public class InputJson
    {
        public GetStats[] getstats;
    }

    [Serializable]
    public class GetStats
    {
        public string type;
        public string id;
        public long timestamp;
        public string trackIdentifier;
        public string kind;
        public float audioLevel;
        public float totalAudioEnergy;
        public float totalSamplesDuration;
        public int echoReturnLoss;
        public float echoReturnLossEnhancement;
        public string fingerprint;
        public string fingerprintAlgorithm;
        public string base64Certificate;
        public string transportId;
        public int payloadType;
        public string mimeType;
        public int clockRate;
        public int channels;
        public string sdpFmtpLine;
        public string label;
        public string protocol;
        public int dataChannelIdentifier;
        public string state;
        public int messagesSent;
        public int bytesSent;
        public int messagesReceived;
        public int bytesReceived;
        public string localCandidateId;
        public string remoteCandidateId;
        public float priority;
        public bool nominated;
        public bool writable;
        public int packetsSent;
        public int packetsReceived;
        public float totalRoundTripTime;
        public int requestsReceived;
        public int requestsSent;
        public int responsesReceived;
        public int responsesSent;
        public int consentRequestsSent;
        public int packetsDiscardedOnSend;
        public int bytesDiscardedOnSend;
        public float currentRoundTripTime;
        public int availableOutgoingBitrate;
        public bool isRemote;
        public string networkType;
        public string ip;
        public string address;
        public int port;
        public string relayProtocol;
        public string candidateType;
        public string url;
        public bool vpn;
        public string networkAdapterType;
        public string mediaSourceId;
        public bool remoteSource;
        public bool ended;
        public bool detached;
        public int frameWidth;
        public int frameHeight;
        public int framesSent;
        public int hugeFramesSent;
        public string streamIdentifier;
        public string[] trackIds;
        public long ssrc;
        public string trackId;
        public string codecId;
        public string mediaType;
        public int retransmittedPacketsSent;
        public int headerBytesSent;
        public int retransmittedBytesSent;
        public int targetBitrate;
        public int nackCount;
        public string remoteId;
        public int framesEncoded;
        public int keyFramesEncoded;
        public float totalEncodeTime;
        public int totalEncodedBytesTarget;
        public int framesPerSecond;
        public float totalPacketSendDelay;
        public string qualityLimitationReason;
        public Qualitylimitationdurations qualityLimitationDurations;
        public int qualityLimitationResolutionChanges;
        public string encoderImplementation;
        public int firCount;
        public int pliCount;
        public int qpSum;
        public int dataChannelsOpened;
        public int dataChannelsClosed;
        public float jitter;
        public int packetsLost;
        public string localId;
        public float roundTripTime;
        public int fractionLost;
        public int roundTripTimeMeasurements;
        public string dtlsState;
        public string selectedCandidatePairId;
        public string localCertificateId;
        public string remoteCertificateId;
        public string tlsVersion;
        public string dtlsCipher;
        public string dtlsRole;
        public string srtpCipher;
        public int selectedCandidatePairChanges;
        public string iceRole;
        public string iceLocalUsernameFragment;
        public string iceState;
        public int width;
        public int height;
        public int frames;
    }

    [Serializable]
    public class Qualitylimitationdurations
    {
        public int bandwidth;
        public int cpu;
        public float none;
        public int other;
    }

    private void OnGUI()
    {
        string logText = _stringBuilder.ToString();
        string line;

        data_.Add(0);

        if (!string.IsNullOrEmpty(logText))
        {
            // いい感じに編集してあげないと JsonUtility が機能しないのでなんとかする
            line = logText.Replace("GetStats: ", "\"getstats\":");
            line = line.Replace("'", "\"");
            line = "{" + line + "}";

            inputJson = JsonUtility.FromJson<InputJson>(line);

            // 値の確認
            // media-source の値
            /*
            EditorGUILayout.LabelField("item type " + inputJson.getstats[0].type);
            EditorGUILayout.LabelField("item type " + inputJson.getstats[0].id);
            EditorGUILayout.LabelField("item type " + inputJson.getstats[0].timestamp);
            EditorGUILayout.LabelField("item type " + inputJson.getstats[0].trackIdentifier);
            EditorGUILayout.LabelField("item type " + inputJson.getstats[0].kind);
            EditorGUILayout.LabelField("item type " + inputJson.getstats[0].audioLevel);
            EditorGUILayout.LabelField("item type " + inputJson.getstats[0].totalAudioEnergy);
            EditorGUILayout.LabelField("item type " + inputJson.getstats[0].totalSamplesDuration);
            EditorGUILayout.LabelField("item type " + inputJson.getstats[0].echoReturnLoss);
            EditorGUILayout.LabelField("item type " + inputJson.getstats[0].echoReturnLossEnhancement);
            */
            Debug.Log(inputJson.getstats[29].jitter);
            data_.Add(inputJson.getstats[29].jitter);
        }

        DrawGraphView();
    }

    private void OnDisable()
    {
        // ログを取得をやめる
        Application.logMessageReceived -= OnReceiveLog;
    }

    private void AddData()
    {
        data_.RemoveAt(0);
        data_.Add(inputJson.getstats[29].jitter);
        Debug.Log(data_);
    }

    private void DrawGraphView()
    {
        Rect drawarea = GUILayoutUtility.GetRect(100f, 200f);


        // グラフの枠を用意
        Handles.DrawSolidRectangleWithOutline(
                new Vector3[]
                {
                    new Vector2(drawarea.x, drawarea.y), new Vector2(drawarea.xMax, drawarea.y),
                    new Vector2(drawarea.xMax, drawarea.yMax), new Vector2(drawarea.x, drawarea.yMax)
                }, new Color(0, 0, 0, 0), Color.white);

        // グラフの線を用意
        Handles.color = new Color(1f, 1f, 1f, 0.5f);

        //　TODO:固定じゃなくて数値で前後するようにしたい
        const int div = 5;

        for (int i = 1; i < div; ++i)
        {
            float x = drawarea.width / div * i;
            float y = drawarea.height / div * i;
            Handles.DrawLine(
                new Vector2(drawarea.x, drawarea.y + y),
                new Vector2(drawarea.xMax, drawarea.y + y));
            Handles.DrawLine(
                new Vector2(drawarea.x + x, drawarea.y),
                new Vector2(drawarea.x + x, drawarea.yMax));

            // TODO:固定じゃなくて数値が必要。表示するグラフに合わせたラベルを用意しないといけない
            Handles.Label(new Vector2(drawarea.x, drawarea.yMax), "0");
            Handles.Label(new Vector2(drawarea.x, drawarea.y), "1");
            var guiSkin = GUI.skin.label;
            guiSkin.fontSize = 10;

        }

        // data
        Handles.color = Color.red;

        var points = new List<Vector3>();
        var max = data_.Max();
        var dx = drawarea.width / data_.Count;
        var dy = drawarea.height / max;

        for (var i = 0; i < data_.Count; ++i)
        {
            var x = drawarea.x + dx * i;
            var y = drawarea.yMax - dy * data_[i];
            points.Add(new Vector2(x, y));
        }

        Handles.DrawAAPolyLine(5f, points.ToArray());

        Handles.color = Color.white;

    }
}
