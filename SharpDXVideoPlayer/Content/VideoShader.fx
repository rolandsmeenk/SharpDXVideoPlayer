// SpriteBatch expects that default texture parameter will have name 'Texture'
Texture2D<float4> Texture : register(t0);

// SpriteBatch expects that default texture sampler parameter will have name 'TextureSampler'
sampler TextureSampler : register(s0);

// SpriteBatch expects that default vertex transform parameter will have name 'MatrixTransform'
row_major float4x4 MatrixTransform;

void SpriteVertexShader(inout float4 color    : COLOR0,
                        inout float2 texCoord : TEXCOORD0,
                        inout float4 position : SV_Position)
{
    position = mul(position, MatrixTransform);
}

float4 SpritePixelShader(float4 color : COLOR0,
                         float2 texCoord : TEXCOORD0) : SV_Target0
{
	float4 pixel = Texture.Sample(TextureSampler, texCoord);
	float gray = (pixel.r + pixel.g + pixel.b) / 3;
	return float4(gray, gray, gray, 1);
}

technique SpriteBatch
{
    pass
    {
        VertexShader = compile vs_2_0 SpriteVertexShader();
        PixelShader  = compile ps_2_0 SpritePixelShader();
    }
}
