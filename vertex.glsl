#version 330

layout(location = 0) in vec2 a_position;

uniform mat4 u_transform;
uniform mat4 u_projection;

void main() {
	gl_Position = u_projection * u_transform * vec4(a_position, 0.0, 1.0);
}