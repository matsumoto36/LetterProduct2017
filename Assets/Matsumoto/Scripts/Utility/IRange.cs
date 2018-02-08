public interface IRange<TValue> where TValue : struct {

	TValue Min { get; set; }
	TValue Max { get; set; }
	TValue Mid { get; }

	TValue Length { get; }
	TValue RandomValue { get; }
}