using System;
using System.Text;

using GLenum = System.UInt32;
using GLshort = System.UInt16;
using GLint = System.Int32;
using GLuint = System.UInt32;
using GLsizei = System.Int32;
using GLfloat = System.Single;

namespace ManagedANGLE
{
    public class SimpleRenderer : IDisposable
    {
        private GLuint mProgram;
        private GLsizei mWindowWidth;
        private GLsizei mWindowHeight;

        private GLint mPositionAttribLocation;
        private GLint mColorAttribLocation;

        private GLint mModelUniformLocation;
        private GLint mViewUniformLocation;
        private GLint mProjUniformLocation;

        private GLuint mVertexPositionBuffer;
        private GLuint mVertexColorBuffer;
        private GLuint mIndexBuffer;

        private int mDrawCount;

        public SimpleRenderer()
        {
            mWindowWidth = 0;
            mWindowHeight = 0;
            mDrawCount = 0;

            // Vertex Shader source
            string vs =
@"uniform mat4 uModelMatrix;
uniform mat4 uViewMatrix;
uniform mat4 uProjMatrix;
attribute vec4 aPosition;
attribute vec4 aColor;
varying vec4 vColor;
void main()
{
    gl_Position = uProjMatrix * uViewMatrix * uModelMatrix * aPosition;
    vColor = aColor;
}";

            // Fragment Shader source
            string fs =
@"precision mediump float;
varying vec4 vColor;
void main()
{
    gl_FragColor = vColor;
}";

            // Set up the shader and its uniform/attribute locations.
            mProgram = CompileProgram(vs, fs);
            mPositionAttribLocation = Gles.glGetAttribLocation(mProgram, "aPosition");
            mColorAttribLocation = Gles.glGetAttribLocation(mProgram, "aColor");
            mModelUniformLocation = Gles.glGetUniformLocation(mProgram, "uModelMatrix");
            mViewUniformLocation = Gles.glGetUniformLocation(mProgram, "uViewMatrix");
            mProjUniformLocation = Gles.glGetUniformLocation(mProgram, "uProjMatrix");

            // Then set up the cube geometry.
            GLfloat[] vertexPositions = new[]
            {
                -1.0f, -1.0f, -1.0f,
                -1.0f, -1.0f,  1.0f,
                -1.0f,  1.0f, -1.0f,
                -1.0f,  1.0f,  1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f,  1.0f,
                 1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f,  1.0f,
            };

            Gles.glGenBuffers(1, out mVertexPositionBuffer);
            Gles.glBindBuffer(Gles.GL_ARRAY_BUFFER, mVertexPositionBuffer);
            Gles.glBufferData(Gles.GL_ARRAY_BUFFER, vertexPositions, Gles.GL_STATIC_DRAW);

            GLfloat[] vertexColors = new[]
            {
                0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 1.0f, 0.0f,
                0.0f, 1.0f, 1.0f,
                1.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 1.0f,
                1.0f, 1.0f, 0.0f,
                1.0f, 1.0f, 1.0f,
            };

            Gles.glGenBuffers(1, out mVertexColorBuffer);
            Gles.glBindBuffer(Gles.GL_ARRAY_BUFFER, mVertexColorBuffer);
            Gles.glBufferData(Gles.GL_ARRAY_BUFFER, vertexColors, Gles.GL_STATIC_DRAW);

            GLshort[] indices = new GLshort[]
            {
                0, 1, 2, // -x
                1, 3, 2,

                4, 6, 5, // +x
                5, 6, 7,

                0, 5, 1, // -y
                0, 4, 5,

                2, 7, 6, // +y
                2, 3, 7,

                0, 6, 4, // -z
                0, 2, 6,

                1, 7, 3, // +z
                1, 5, 7,
            };

            Gles.glGenBuffers(1, out mIndexBuffer);
            Gles.glBindBuffer(Gles.GL_ELEMENT_ARRAY_BUFFER, mIndexBuffer);
            Gles.glBufferData(Gles.GL_ELEMENT_ARRAY_BUFFER, indices, Gles.GL_STATIC_DRAW);
        }

        public void Dispose()
        {
            if (mProgram != 0)
            {
                Gles.glDeleteProgram(mProgram);
                mProgram = 0;
            }

            if (mVertexPositionBuffer != 0)
            {
                Gles.glDeleteBuffers(1, ref mVertexPositionBuffer);
                mVertexPositionBuffer = 0;
            }

            if (mVertexColorBuffer != 0)
            {
                Gles.glDeleteBuffers(1, ref mVertexColorBuffer);
                mVertexColorBuffer = 0;
            }

            if (mIndexBuffer != 0)
            {
                Gles.glDeleteBuffers(1, ref mIndexBuffer);
                mIndexBuffer = 0;
            }
        }

        public void Draw()
        {
            Gles.glEnable(Gles.GL_DEPTH_TEST);
            Gles.glClear(Gles.GL_COLOR_BUFFER_BIT | Gles.GL_DEPTH_BUFFER_BIT);

            if (mProgram == 0)
                return;

            Gles.glUseProgram(mProgram);

            Gles.glBindBuffer(Gles.GL_ARRAY_BUFFER, mVertexPositionBuffer);
            Gles.glEnableVertexAttribArray((GLuint)mPositionAttribLocation);
            Gles.glVertexAttribPointer((GLuint)mPositionAttribLocation, 3, Gles.GL_FLOAT, Gles.GL_FALSE, 0, IntPtr.Zero);

            Gles.glBindBuffer(Gles.GL_ARRAY_BUFFER, mVertexColorBuffer);
            Gles.glEnableVertexAttribArray((GLuint)mColorAttribLocation);
            Gles.glVertexAttribPointer((GLuint)mColorAttribLocation, 3, Gles.GL_FLOAT, Gles.GL_FALSE, 0, IntPtr.Zero);

            float[,] modelMatrix = MathHelpers.SimpleModelMatrix((float)mDrawCount / 50.0f);
            Gles.glUniformMatrix4fv(mModelUniformLocation, 1, Gles.GL_FALSE, modelMatrix);

            float[,] viewMatrix = MathHelpers.SimpleViewMatrix();
            Gles.glUniformMatrix4fv(mViewUniformLocation, 1, Gles.GL_FALSE, viewMatrix);

            float[,] projectionMatrix = MathHelpers.SimpleProjectionMatrix((float)mWindowWidth / (float)mWindowHeight);
            Gles.glUniformMatrix4fv(mProjUniformLocation, 1, Gles.GL_FALSE, projectionMatrix);

            // Draw 36 indices: six faces, two triangles per face, 3 indices per triangle
            Gles.glBindBuffer(Gles.GL_ELEMENT_ARRAY_BUFFER, mIndexBuffer);
            Gles.glDrawElements(Gles.GL_TRIANGLES, (6 * 2) * 3, Gles.GL_UNSIGNED_SHORT, IntPtr.Zero);

            mDrawCount += 1;
        }

        public void UpdateWindowSize(GLsizei width, GLsizei height)
        {
            Gles.glViewport(0, 0, width, height);
            mWindowWidth = width;
            mWindowHeight = height;
        }

        private static GLuint CompileShader(GLenum type, string source)
        {
            GLuint shader = Gles.glCreateShader(type);

            Gles.glShaderSource(shader, 1, new[] { source }, new[] { source.Length });
            Gles.glCompileShader(shader);

            Gles.glGetShaderiv(shader, Gles.GL_COMPILE_STATUS, out GLint compileResult);

            if (compileResult == 0)
            {
                Gles.glGetShaderiv(shader, Gles.GL_INFO_LOG_LENGTH, out GLint infoLogLength);

                StringBuilder infoLog = new StringBuilder(infoLogLength);
                Gles.glGetShaderInfoLog(shader, (GLsizei)infoLog.Capacity, out GLsizei length, infoLog);

                string errorMessage = "Shader compilation failed: " + infoLog;
                throw new Exception(errorMessage);
            }

            return shader;
        }

        private static GLuint CompileProgram(string vsSource, string fsSource)
        {
            GLuint program = Gles.glCreateProgram();

            if (program == 0)
            {
                throw new Exception("Program creation failed");
            }

            GLuint vs = CompileShader(Gles.GL_VERTEX_SHADER, vsSource);
            GLuint fs = CompileShader(Gles.GL_FRAGMENT_SHADER, fsSource);

            if (vs == 0 || fs == 0)
            {
                Gles.glDeleteShader(fs);
                Gles.glDeleteShader(vs);
                Gles.glDeleteProgram(program);
                return 0;
            }

            Gles.glAttachShader(program, vs);
            Gles.glDeleteShader(vs);

            Gles.glAttachShader(program, fs);
            Gles.glDeleteShader(fs);

            Gles.glLinkProgram(program);

            Gles.glGetProgramiv(program, Gles.GL_LINK_STATUS, out GLint linkStatus);

            if (linkStatus == 0)
            {
                Gles.glGetProgramiv(program, Gles.GL_INFO_LOG_LENGTH, out GLint infoLogLength);

                StringBuilder infoLog = new StringBuilder(infoLogLength);
                Gles.glGetProgramInfoLog(program, (GLsizei)infoLog.Capacity, out GLsizei length, infoLog);

                string errorMessage = "Program link failed: " + infoLog;
                throw new Exception(errorMessage);
            }

            return program;
        }
    }
}
