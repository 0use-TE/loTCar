using nanoFramework.Hardware.Esp32;
using System;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Diagnostics;

namespace LowerComputer.Services
{
	public class MotorService
	{
		private readonly GpioController gpioController;
		private readonly PwmChannel pwm_ENA;
		private readonly PwmChannel pwm_ENB;
		public const int ENA = 16; // PWM for left motor speed- 
		public const int IN1 = 17; // Left motor direction 1
		public const int IN2 = 5;  // Left motor direction 2
		public const int IN3 = 19; // Right motor direction 1
		public const int IN4 = 18; // Right motor direction 2
		public const int ENB = 21; // PWM for right motor speed
		public MotorService()
		{
			try
			{
				gpioController = new GpioController();
				// Configure PWM pins
				Configuration.SetPinFunction(ENA, DeviceFunction.PWM1);
				Configuration.SetPinFunction(ENB, DeviceFunction.PWM2);

				// Configure GPIO pins for motor direction
				gpioController.OpenPin(IN1, PinMode.Output);
				gpioController.OpenPin(IN2, PinMode.Output);
				gpioController.OpenPin(IN3, PinMode.Output);
				gpioController.OpenPin(IN4, PinMode.Output);

				// Initialize PWM channels
				pwm_ENA = PwmChannel.CreateFromPin(ENA, frequency:1000);
				pwm_ENB = PwmChannel.CreateFromPin(ENB, frequency: 1000);

				// Start PWM
				pwm_ENA.Start();
				pwm_ENB.Start();

				// Initialize motors to stopped state
				Stop();
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"MotorService initialization failed: {ex.Message}");
				throw;
			}
		}

		/// <summary>
		/// Sets the speed for both motors (0 to 100).
		/// </summary>
		/// <param name="speed">Speed percentage (0 to 100).</param>
		public void SetSpeed(int speed)
		{
			if (speed < 0 || speed > 100)
			{
				throw new ArgumentOutOfRangeException(nameof(speed), "Speed must be between 0 and 100.");
			}

			// Convert speed percentage to PWM duty cycle (0.0 to 1.0)
			double dutyCycle = speed / 100.0;
			pwm_ENA.DutyCycle = dutyCycle;
			pwm_ENB.DutyCycle = dutyCycle;
		}

		/// <summary>
		/// Moves the motors forward.
		/// </summary>
		/// <param name="speed">Speed percentage (0 to 100).</param>
		public void Forward(int speed)
		{
			SetSpeed(speed);

			// Left motor forward: IN1 = High, IN2 = Low
			gpioController.Write(IN1, PinValue.High);
			gpioController.Write(IN2, PinValue.Low);

			// Right motor forward: IN3 = High, IN4 = Low
			gpioController.Write(IN3, PinValue.High);
			gpioController.Write(IN4, PinValue.Low);

			Debug.WriteLine($"Moving forward at speed {speed}%");
		}

		/// <summary>
		/// Moves the motors backward.
		/// </summary>
		/// <param name="speed">Speed percentage (0 to 100).</param>
		public void Backward(int speed)
		{

			// Set speed
			SetSpeed(speed);

			// Left motor backward: IN1 = Low, IN2 = High
			gpioController.Write(IN1, PinValue.Low);
			gpioController.Write(IN2, PinValue.High);

			// Right motor backward: IN3 = Low, IN4 = High
			gpioController.Write(IN3, PinValue.Low);
			gpioController.Write(IN4, PinValue.High);
			Debug.WriteLine($"Moving backward at speed {speed}%");
		}

		/// <summary>
		/// Turns the motors left (right motor forward, left motor stopped).
		/// </summary>
		/// <param name="speed">Speed percentage (0 to 100).</param>
		public void TurnLeft(int speed)
		{

			// Set speed (only for right motor)
			pwm_ENA.DutyCycle = 0; // Stop left motor
			pwm_ENB.DutyCycle = speed / 100.0; // Right motor speed

			// Left motor stopped: IN1 = Low, IN2 = Low
			gpioController.Write(IN1, PinValue.Low);
			gpioController.Write(IN2, PinValue.Low);

			// Right motor forward: IN3 = High, IN4 = Low
			gpioController.Write(IN3, PinValue.High);
			gpioController.Write(IN4, PinValue.Low);


			Debug.WriteLine($"Turning left at speed {speed}%");
		}

		/// <summary>
		/// Turns the motors right (left motor forward, right motor stopped).
		/// </summary>
		/// <param name="speed">Speed percentage (0 to 100).</param>
		public void TurnRight(int speed)
		{

			// Set speed (only for left motor)
			pwm_ENA.DutyCycle = speed / 100.0; // Left motor speed
			pwm_ENB.DutyCycle = 0; // Stop right motor

			// Left motor forward: IN1 = High, IN2 = Low
			gpioController.Write(IN1, PinValue.High);
			gpioController.Write(IN2, PinValue.Low);

			// Right motor stopped: IN3 = Low, IN4 = Low
			gpioController.Write(IN3, PinValue.Low);
			gpioController.Write(IN4, PinValue.Low);


			Debug.WriteLine($"Turning right at speed {speed}%");
		}

		/// <summary>
		/// Stops both motors.
		/// </summary>
		public void Stop()
		{
			// Stop both motors: set all direction pins to Low
			gpioController.Write(IN1, PinValue.Low);
			gpioController.Write(IN2, PinValue.Low);
			gpioController.Write(IN3, PinValue.Low);
			gpioController.Write(IN4, PinValue.Low);

			// Set PWM to 0
			pwm_ENA.DutyCycle = 0;
			pwm_ENB.DutyCycle = 0;
			Debug.WriteLine("Motors stopped");
		}

		/// <summary>
		/// Disposes resources used by the MotorService.
		/// </summary>
		public void Dispose()
		{
			pwm_ENA?.Stop();
			pwm_ENB?.Stop();
			pwm_ENA?.Dispose();
			pwm_ENB?.Dispose();
			gpioController?.Dispose();
			Debug.WriteLine("MotorService disposed");
		}
	}
}