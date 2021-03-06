using UnityEngine;
using System;
using live2d;
using live2d.framework;

[ExecuteInEditMode]
public class SimpleModel : MonoBehaviour
{
    public TextAsset mocFile;
    public TextAsset physicsFile;
    public Texture2D[] textureFiles;

    private Live2DModelUnity live2DModel;
    private EyeBlinkMotion eyeBlink = new EyeBlinkMotion();
    private L2DTargetPoint dragMgr = new L2DTargetPoint();
    private L2DPhysics physics;
    private Matrix4x4 live2DCanvasPos;

    float mousePositionX, mousePositionY;
    public float MousePostionX
    {
        get { return mousePositionX; }
        set { mousePositionX = value; }
    }
    public float MousePostionY
    {
        get { return mousePositionY; }
        set { mousePositionY = value; }
    }

    float matsutakeSize = -1f;
    public float MatsutakeSize
    {
        get { return matsutakeSize; }
        set { matsutakeSize = value; }
    }

    float haneMovePalam;
    float haneMoveRad;

    public event Func<bool> isGameStart;
    public event Func<bool> isGameEnd;
    public event Func<bool> isGameClear;

    void Start()
    {
        Live2D.init();
        load();
    }


    void load()
    {
        live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);

        // Live2Dのレンダーモード変更
        live2DModel.setRenderMode(Live2D.L2D_RENDER_DRAW_MESH);

        for (int i = 0; i < textureFiles.Length; i++)
        {
            live2DModel.setTexture(i, textureFiles[i]);
        }

        float modelWidth = live2DModel.getCanvasWidth();
        live2DCanvasPos = Matrix4x4.Ortho(0, modelWidth, modelWidth, 0, -50.0f, 50.0f);

        if (physicsFile != null) physics = L2DPhysics.load(physicsFile.bytes);

        // 松茸の大きさ(0.0f:小～1.0f:大)
        matsutakeSize = -1f;

        //涙の大きさ(0.0f:小～1.0f:大)
        live2DModel.setParamFloat("PARAM_NAMIDA_SCALE_RIGHT", 1f);
        live2DModel.setParamFloat("PARAM_NAMIDA_SCALE_LEFT", 1f);
    }


    void Update()
    {
        if (live2DModel == null) load();
        live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);

        if (!Application.isPlaying)
        {
            live2DModel.update();
            return;
        }

        float min = Screen.height / 6f;
        float max = Screen.height / 2.5f;

        var pos = Input.mousePosition;
        //        dragMgr.Set(pos.x / Screen.width * 2 - 1, pos.y / Screen.height * 4 - 1);
        //dragMgr.Set(pos.x / Screen.width * 2 - 1, Mathf.Clamp((pos.y - min) * 2 / (max - min) - 1, -1, 1));

        //dragMgr.update();
        
        if (!isGameEnd())
        {
            mousePositionX = pos.x / Screen.width * 2 - 1;
            mousePositionY = Mathf.Clamp((pos.y - min) * 2 / (max - min) - 1, -1, 1);
        }

        // 角度XY
        live2DModel.setParamFloat("PARAM_ANGLE_X", mousePositionX * 30);
        live2DModel.setParamFloat("PARAM_ANGLE_Y", mousePositionY * 30);

        // 体X
        live2DModel.setParamFloat("PARAM_BODY_ANGLE_X", mousePositionX * -10);

        // 目玉
        live2DModel.setParamFloat("PARAM_EYE_BALL_X", mousePositionX);
        live2DModel.setParamFloat("PARAM_EYE_BALL_Y", mousePositionY);
        
        // アホ毛(上)
        live2DModel.setParamFloat("PARAM_AHOGE_TOP_ROTATE", mousePositionY);

        //アホ毛(左上)
        live2DModel.setParamFloat("PARAM_AHOGE_LEFT_ROTATE", mousePositionY);

        // 髪揺れ横(-1.0f～1.0f)
        live2DModel.setParamFloat("PARAM_HAIR_SIDE", -mousePositionX);

        // 口の開閉(0.0f～1.0f)
        live2DModel.setParamFloat("PARAM_MOUTH_OPEN_Y", -mousePositionY);

        // きのこ上下(-1.0f～1.0f)
        live2DModel.setParamFloat("PARAM_MATSUTAKE_Y", mousePositionY);

        // きのこ大きさ(0.0f～1.0f)
        live2DModel.setParamFloat("PARAM_MATSUTAKE_SCALE", matsutakeSize);

        // 羽
        // 後で修正する
        // 口に入れられたとき、一瞬ぴくっとなって元に戻るようにする

        // 羽右1 上下
        live2DModel.setParamFloat("PARAM_HANE_RIGHT_01_Y", haneMovePalam);
        // 羽右2 上下
        live2DModel.setParamFloat("PARAM_HANE_RIGHT_02_Y", haneMovePalam);
        // 羽左1 上下
        live2DModel.setParamFloat("PARAM_HANE_LEFT_01_Y", haneMovePalam);
        // 羽左2 上下
        live2DModel.setParamFloat("PARAM_HANE_LEFT_02_Y", haneMovePalam);


        if (isGameClear())
        {
            haneMovePalam = Mathf.Min(haneMovePalam + 0.2f, 1f);
        }
        else
        {
            float haneTime = UtSystem.getUserTimeMSec() / 2000.0f;
            haneMovePalam = Mathf.Sin(haneTime);
        }

        /*
        if (dragMgr.getY() >= 0.99f && isGameStart())
        {
            if (haneMovePalam <= 0.95f)
            {
                if (haneMoveRad >= Mathf.PI / 2.0f && haneMoveRad <= Mathf.PI * 1.5f)
                {
                    haneMoveRad -= 0.6f;
                }
                else
                {
                    haneMoveRad += 0.6f;
                }
            }
        }
        else
        {
            haneMoveRad += 0.08f;
        }

        if (haneMoveRad >= 2 * Mathf.PI)
        {
            haneMoveRad -= 2 * Mathf.PI;
        }

        haneMovePalam = Mathf.Sin(haneMoveRad);
        //Debug.Log("hane : " + haneMoveRad + ", " + haneMovePalam);
        */

        double timeSec = UtSystem.getUserTimeMSec() / 1000.0;
        double t = timeSec * 2 * Math.PI;
        live2DModel.setParamFloat("PARAM_BREATH", (float)(0.5f + 0.5f * Math.Sin(t / 3.0)));

        eyeBlink.setParam(live2DModel);

        if (physics != null) physics.updateParam(live2DModel);

        live2DModel.update();

        live2DModel.draw();
    }


    public void OnRenderObject()
    {
        if (live2DModel == null) load();
        if (live2DModel.getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH_NOW) live2DModel.draw();
    }

    // ゲーム開始時に呼び出し（最初はモデルに含まれるきのこは非表示）
    public void GameStart()
    {
        // 松茸の大きさ(0.0f:小～1.0f:大)
        matsutakeSize = 0f;
    }

    public float GetDragManagerY()
    {
        return dragMgr.getY();
    }

    public void SetMatsutakeScale(float scale)
    {
        matsutakeSize = Mathf.Clamp(scale, 0, 1);
    }

    public void SetFaceRed(float face)
    {
        live2DModel.setParamFloat("PARAM_FACE_RED", face);
    }
}