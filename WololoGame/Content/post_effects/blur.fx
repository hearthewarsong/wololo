float4x4 World;
float4x4 View;
float4x4 Projection;

// TODO: add effect parameters here.

texture2D originalTex;
texture2D blurredTex;
texture2D focusTex;
float2 screenSize;

sampler originalTexSampler = sampler_state
{
	Texture = <originalTex>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};

sampler blurredTexSampler = sampler_state
{
	Texture = <blurredTex>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};

sampler focusTexSampler = sampler_state
{
	Texture = <focusTex>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};

struct VertexShaderInput
{
    float3 Position : SV_Position;
	float2 TexCoord : TEXCOORD0;

    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : SV_Position;
	float2 TexCoord : TEXCOORD0;

    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct AdjacentTexel
{
    float2 texCoord1 : TEXCOORD1;
    float2 texCoord2 : TEXCOORD2;
    float2 texCoord3 : TEXCOORD3;
    float2 texCoord4 : TEXCOORD4;
    float2 texCoord5 : TEXCOORD5;
    float2 texCoord6 : TEXCOORD6;
    float2 texCoord7 : TEXCOORD7;
    float2 texCoord8 : TEXCOORD8;
    float2 texCoord9 : TEXCOORD9;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	output.Position = float4(input.Position, 1);
	output.TexCoord = float4(input.TexCoord, 1, 1);

	return output;
}

float4 PixelShaderBlurH(VertexShaderOutput input) : COLOR0
{
	AdjacentTexel texel;
    float texelSize;
    float weight0, weight1, weight2, weight3, weight4;
    float normalization;
    float4 color;

    // Determine the floating point size of a texel for a screen.
    texelSize.x = 1.0f / screenSize.x;

    // Create the weights that each neighbor pixel will contribute to the blur.
	// I use normal distribution for a Gaussian blur
    weight0 = 1.0f;
    weight1 = 0.92f;
    weight2 = 0.7f;
    weight3 = 0.38f;
    weight4 = 0.2f;

    // Create a normalized value to average the weights out a bit.
    normalization = weight0 + 2.0f * (weight1 + weight2 + weight3 + weight4);

    // Normalize the weights.
    weight0 = weight0 / normalization;
    weight1 = weight1 / normalization;
    weight2 = weight2 / normalization;
    weight3 = weight3 / normalization;
    weight4 = weight4 / normalization;

    // Initialize the color to black.
    color = float4(0.0f, 0.0f, 0.0f, 0.0f);

    // Create UV coordinates for the pixel and its four horizontal neighbors on either side.
    texel.texCoord1 = input.TexCoord + float2(texelSize * -4.0f, 0.0f);
    texel.texCoord2 = input.TexCoord + float2(texelSize * -3.0f, 0.0f);
    texel.texCoord3 = input.TexCoord + float2(texelSize * -2.0f, 0.0f);
    texel.texCoord4 = input.TexCoord + float2(texelSize * -1.0f, 0.0f);
    texel.texCoord5 = input.TexCoord;
    texel.texCoord6 = input.TexCoord + float2(texelSize *  1.0f, 0.0f);
    texel.texCoord7 = input.TexCoord + float2(texelSize *  2.0f, 0.0f);
    texel.texCoord8 = input.TexCoord + float2(texelSize *  3.0f, 0.0f);
    texel.texCoord9 = input.TexCoord + float2(texelSize *  4.0f, 0.0f);

    // Add the nine horizontal pixels to the color by the specific weight of each.
    color += tex2D(originalTexSampler, texel.texCoord1) * weight4;
    color += tex2D(originalTexSampler, texel.texCoord2) * weight3;
    color += tex2D(originalTexSampler, texel.texCoord3) * weight2;
    color += tex2D(originalTexSampler, texel.texCoord4) * weight1;
    color += tex2D(originalTexSampler, texel.texCoord5) * weight0;
    color += tex2D(originalTexSampler, texel.texCoord6) * weight1;
    color += tex2D(originalTexSampler, texel.texCoord7) * weight2;
    color += tex2D(originalTexSampler, texel.texCoord8) * weight3;
    color += tex2D(originalTexSampler, texel.texCoord9) * weight4;

    // Set the alpha channel to one.
    color.a = 1.0f;

    return color;
}

float4 PixelShaderBlurV(VertexShaderOutput input) : COLOR0
{
	AdjacentTexel texel;
    float texelSize;
    float weight0, weight1, weight2, weight3, weight4;
    float normalization;
    float4 color;

    // Determine the floating point size of a texel for a screen.
    texelSize = 1.0f / screenSize.y;

    // Create the weights that each neighbor pixel will contribute to the blur.
	// I use normal distribution for a semi-Gaussian blur
    weight0 = 1.0f;
    weight1 = 0.92f;
    weight2 = 0.7f;
    weight3 = 0.38f;
    weight4 = 0.2f;

    // Create a normalized value to average the weights out a bit.
    normalization = weight0 + 2.0f * (weight1 + weight2 + weight3 + weight4);

    // Normalize the weights.
    weight0 = weight0 / normalization;
    weight1 = weight1 / normalization;
    weight2 = weight2 / normalization;
    weight3 = weight3 / normalization;
    weight4 = weight4 / normalization;

    // Initialize the color to black.
    color = float4(0.0f, 0.0f, 0.0f, 0.0f);

    // Create UV coordinates for the pixel and its four vertical neighbors on either side.
    texel.texCoord1 = input.TexCoord + float2(0.0f, texelSize * -4.0f);
    texel.texCoord2 = input.TexCoord + float2(0.0f, texelSize * -3.0f);
    texel.texCoord3 = input.TexCoord + float2(0.0f, texelSize * -2.0f);
    texel.texCoord4 = input.TexCoord + float2(0.0f, texelSize * -1.0f);
    texel.texCoord5 = input.TexCoord;
    texel.texCoord6 = input.TexCoord + float2(0.0f, texelSize *  1.0f);
    texel.texCoord7 = input.TexCoord + float2(0.0f, texelSize *  2.0f);
    texel.texCoord8 = input.TexCoord + float2(0.0f, texelSize *  3.0f);
    texel.texCoord9 = input.TexCoord + float2(0.0f, texelSize *  4.0f);

    // Add the nine horizontal pixels to the color by the specific weight of each.
    color += tex2D(originalTexSampler, texel.texCoord1) * weight4;
    color += tex2D(originalTexSampler, texel.texCoord2) * weight3;
    color += tex2D(originalTexSampler, texel.texCoord3) * weight2;
    color += tex2D(originalTexSampler, texel.texCoord4) * weight1;
    color += tex2D(originalTexSampler, texel.texCoord5) * weight0;
    color += tex2D(originalTexSampler, texel.texCoord6) * weight1;
    color += tex2D(originalTexSampler, texel.texCoord7) * weight2;
    color += tex2D(originalTexSampler, texel.texCoord8) * weight3;
    color += tex2D(originalTexSampler, texel.texCoord9) * weight4;

    // Set the alpha channel to one.
    color.a = 1.0f;

    return color;
}

float4 PixelShaderUpSample(VertexShaderOutput input) : COLOR0
{
	return tex2D( originalTexSampler, input.TexCoord.xy);
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_4_1 VertexShaderFunction();
        PixelShader = compile ps_4_1 PixelShaderBlurH();
    }

    pass Pass2
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_4_1 VertexShaderFunction();
        PixelShader = compile ps_4_1 PixelShaderBlurV();
    }

    pass Pass3
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_4_1 VertexShaderFunction();
        PixelShader = compile ps_4_1 PixelShaderUpSample();
    }
}